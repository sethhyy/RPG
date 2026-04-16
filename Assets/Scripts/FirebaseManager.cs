using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    /// <summary>True after Firestore DefaultInstance is obtained (cloud save).</summary>
    public bool IsFirebaseReady { get; private set; }

    private FirebaseFirestore db;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance != this) return;
        TryInitFirestore();
    }

    void TryInitFirestore()
    {
        try
        {
            db = FirebaseFirestore.DefaultInstance;
            IsFirebaseReady = true;
            Debug.Log("[FirebaseManager] Firestore ready.");
        }
        catch (System.Exception e)
        {
            IsFirebaseReady = false;
            Debug.LogWarning("[FirebaseManager] Firestore unavailable (continuing without cloud sync): " + e.Message);
        }
    }

    public void LoadAndStartGame()
    {
        if (!IsFirebaseReady)
        {
            StartCoroutine(LoadAndStartGameWhenReady());
            return;
        }

        DoLoadAndStartGame();
    }

    IEnumerator LoadAndStartGameWhenReady()
    {
        float t = 0f;
        const float maxWait = 2f;
        while (!IsFirebaseReady && t < maxWait)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (!IsFirebaseReady)
        {
            Debug.LogWarning("[FirebaseManager] Firestore not ready — loading CharacterSelect without cloud sync.");
            DoLoadAndStartGameWithoutFirestore();
            yield break;
        }

        DoLoadAndStartGame();
    }

    /// <summary>Continue after sign-in when Firestore never became ready (no Google Play dependency).</summary>
    void DoLoadAndStartGameWithoutFirestore()
    {
        if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("LoginScene");
            return;
        }

        RunDeferredSceneLoad("CharacterSelect");
    }

    void DoLoadAndStartGame()
    {
        if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.LogError("No user logged in!");
            SceneManager.LoadScene("LoginScene");
            return;
        }

        string uId = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        Debug.Log("FirebaseManager: Fetching Cloud Data for UID: " + uId);

        if (db == null)
        {
            Debug.LogError("[FirebaseManager] Firestore not initialized.");
            RunDeferredSceneLoad("CharacterSelect");
            return;
        }

        db.Collection("users").Document(uId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Firebase user document fetch failed: " + task.Exception?.GetBaseException().Message);
                if (SceneManager.GetActiveScene().name == "LoginScene")
                    RunDeferredSceneLoad("CharacterSelect");
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Debug.Log("New user: no Firestore profile yet. Loading CharacterSelect.");
                RunDeferredSceneLoad("CharacterSelect");
                return;
            }

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager missing; cannot apply cloud save. Loading CharacterSelect.");
                RunDeferredSceneLoad("CharacterSelect");
                return;
            }

            var p = GameManager.Instance.playerData;
            p.gold = snapshot.ContainsField("gold") ? snapshot.GetValue<int>("gold") : 0;
            p.CurrentHearts = snapshot.ContainsField("currentHearts") ? snapshot.GetValue<int>("currentHearts") : 5;
            p.CharacterGender = snapshot.ContainsField("characterGender") ? snapshot.GetValue<string>("characterGender") : "Lalaki";
            p.PlayerName = snapshot.ContainsField("playerName") ? snapshot.GetValue<string>("playerName") : "Player";

            string map = snapshot.ContainsField("currentMap") ? snapshot.GetValue<string>("currentMap") : "LabasNgBahay";

            Debug.Log($"Data synced. Loading {map}");
            RunDeferredSceneLoad(map);
        });
    }

    /// <summary>
    /// Firebase's ContinueWithOnMainThread can run outside a normal Unity frame boundary where
    /// synchronous SceneManager.LoadScene is ignored. Defer one frame and use LoadSceneAsync.
    /// </summary>
    void RunDeferredSceneLoad(string sceneName)
    {
        if (Instance == null || !isActiveAndEnabled)
        {
            Debug.LogError("[FirebaseManager] Cannot defer scene load — Instance or component inactive. Loading immediately.");
            SceneManager.LoadScene(sceneName);
            return;
        }

        StartCoroutine(DeferredSceneLoadRoutine(sceneName));
    }

    static int FindBuildIndexBySceneName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (string.IsNullOrEmpty(path)) continue;
            string name = Path.GetFileNameWithoutExtension(path);
            if (string.Equals(name, sceneName, System.StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }

    IEnumerator DeferredSceneLoadRoutine(string sceneName)
    {
        yield return null;

        int buildIndex = FindBuildIndexBySceneName(sceneName);
        AsyncOperation op = buildIndex >= 0
            ? SceneManager.LoadSceneAsync(buildIndex)
            : SceneManager.LoadSceneAsync(sceneName);

        if (op == null)
        {
            Debug.LogError($"[FirebaseManager] LoadSceneAsync failed for '{sceneName}'. Add it to File > Build Settings > Scenes In Build.");
            yield break;
        }

        yield return op;
        Debug.Log($"[FirebaseManager] Scene load finished. Active scene: '{SceneManager.GetActiveScene().name}'");
    }

    public void SaveCurrentProgress()
    {
        if (!IsFirebaseReady || db == null) return;
        if (GameManager.Instance == null || Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser == null) return;

        string uId = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var p = GameManager.Instance.playerData;

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "gold", p.gold },
            { "currentHearts", p.CurrentHearts },
            { "characterGender", p.CharacterGender },
            { "playerName", p.PlayerName },
            { "currentMap", SceneManager.GetActiveScene().name }
        };

        db.Collection("users").Document(uId).SetAsync(updates, SetOptions.MergeAll);
    }
}