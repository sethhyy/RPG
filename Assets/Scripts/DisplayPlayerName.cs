using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayPlayerName : MonoBehaviour
{
    public TMP_Text playerNameText;

    void Awake()
    {
        // Safety check
        if (playerNameText == null)
        {
            Debug.LogError("DisplayPlayerName: playerNameText is NOT assigned!");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        UpdateName();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateName();
    }

    void UpdateName()
    {
        if (playerNameText == null)
            return;

        if (GameManager.Instance == null)
        {
            playerNameText.text = "Player";
            Debug.LogWarning("GameManager.Instance is NULL");
            return;
        }

        if (GameManager.Instance.playerData == null)
        {
            playerNameText.text = "Player";
            Debug.LogWarning("playerData is NULL");
            return;
        }

        if (string.IsNullOrEmpty(GameManager.Instance.playerData.PlayerName))
        {
            playerNameText.text = "Player";
            return;
        }

        playerNameText.text = GameManager.Instance.playerData.PlayerName;
    }
}
