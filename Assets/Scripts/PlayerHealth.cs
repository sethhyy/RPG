using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public int maxHearts = 5;
    public int currentHearts;

    [Header("UI")]
    public Image[] heartImages;

    public int CurrentHealth { get; internal set; }

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
            return;
        }
    }

    private void Start()
    {
        // --- LOAD HEALTH ON START ---
        // We get the email from the GameManager and load the hearts from PlayerPrefs
        string email = GameManager.Instance.LoggedInEmail;

        LocalAccountData.LoadHearts(email, out int loadedCurrent, out int loadedMax);

        maxHearts = loadedMax;
        currentHearts = loadedCurrent;
        CurrentHealth = currentHearts;

        UpdateHeartsUI();
    }

    public void TakeDamage(int amount = 1)
    {
        if (currentHearts <= 0) return;

        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        CurrentHealth = currentHearts;

        UpdateHeartsUI();

        // --- SAVE HEALTH IMMEDIATELY ---
        SaveCurrentHealth();

        if (currentHearts == 0)
        {
            GameOver();
        }
    }

    // New helper method to trigger the save
    private void SaveCurrentHealth()
    {
        string email = GameManager.Instance.LoggedInEmail;

        // Update the LocalAccountData (PlayerPrefs)
        LocalAccountData.SaveHearts(email, currentHearts, maxHearts);

        // Also update the GameManager's temporary data so they stay in sync
        GameManager.Instance.playerData.CurrentHearts = currentHearts;

        // Force Unity to write to the disk
        PlayerPrefs.Save();
    }

    public void UpdateHeartsUI()
    {
        if (heartImages == null) return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].gameObject.SetActive(i < currentHearts);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over – Hearts depleted");
    }

    public void SetHeartUI(Image[] newHeartImages)
    {
        heartImages = newHeartImages;
        UpdateHeartsUI();
    }
}
