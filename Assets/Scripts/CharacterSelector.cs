using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text statusText;
    public string selectedGender = "Lalaki";

    public void SelectLalaki() { selectedGender = "Lalaki"; }
    public void SelectBabae() { selectedGender = "Babae"; }

    /// <summary>Wired to GenderDropdown OnValueChanged (int). Order in scene: 0 = Babae, 1 = Lalaki.</summary>
    public void ChangeGender(int dropdownIndex)
    {
        selectedGender = dropdownIndex == 0 ? "Babae" : "Lalaki";
    }

    public void SaveAndStart()
    {
        if (playerNameInput == null)
        {
            Debug.LogError("CharacterSelector: assign Player Name Input in the Inspector.");
            return;
        }

        string name = playerNameInput.text != null ? playerNameInput.text.Trim() : "";
        if (string.IsNullOrEmpty(name))
        {
            if (statusText != null)
                statusText.text = "Paki-lagay ang pangalan!";
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("CharacterSelector: GameManager.Instance is null.");
            if (statusText != null)
                statusText.text = "May error. Mag-login muli.";
            return;
        }

        GameManager.Instance.playerData.PlayerName = name;
        GameManager.Instance.playerData.CharacterGender = selectedGender;

        SceneManager.LoadScene("LabasNgBahay");

        if (FirebaseManager.Instance != null)
            FirebaseManager.Instance.SaveCurrentProgress();
    }
}