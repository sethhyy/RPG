using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public static string NextSpawnID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hanapin ang player sa scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("SpawnManager: No Player object found in scene.");
            return;
        }

        if (string.IsNullOrEmpty(NextSpawnID))
        {
            Debug.Log("SpawnManager: No spawn ID set. Player stays at current position.");
            return;
        }

        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        bool foundSpawn = false;

        foreach (SpawnPoint sp in spawnPoints)
        {
            if (sp.spawnID == NextSpawnID)
            {
                // FORCE POSITION: Siguraduhin na Z = 0
                player.transform.position = new Vector3(sp.transform.position.x, sp.transform.position.y, 0f);

                // Stop any sliding physics
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.linearVelocity = Vector2.zero;

                Debug.Log($"SpawnManager: Player moved to {NextSpawnID} at {player.transform.position}");
                foundSpawn = true;
                break;
            }
        }

        if (!foundSpawn)
        {
            Debug.LogError($"SpawnManager: SpawnID '{NextSpawnID}' not found in this scene!");
        }

        // Reset ID for the next transition
        NextSpawnID = null;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}