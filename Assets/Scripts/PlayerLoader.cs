using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerLoader : MonoBehaviour
{
    public TMP_Text nameTag;
    private static PlayerLoader instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load aesthetics and data only
        LoadFromGameManager();
        StartCoroutine(DisableCollisionTemporarily());
    }

    private void LoadFromGameManager()
    {
        if (GameManager.Instance != null && GameManager.Instance.playerData != null)
        {
            PlayerData data = GameManager.Instance.playerData;

            if (nameTag != null)
                nameTag.text = data.PlayerName;

            var setup = GetComponent<PlayerSetup>();
            if (setup != null)
                setup.ApplyAppearance(data);

            Debug.Log("PlayerLoader: Appearance and Name applied.");
        }
    }

    IEnumerator DisableCollisionTemporarily()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            yield return new WaitForSeconds(0.2f); // Binagalan konti para sure na tapos na ang spawn
            col.enabled = true;
        }
    }
}