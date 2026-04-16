using UnityEngine;

public class ToggleDictionary : MonoBehaviour
{
    public GameObject dictionaryPanel;

    // Toggle method
    public void TogglePanel()
    {
        if (dictionaryPanel != null)
        {
            bool isActive = dictionaryPanel.activeSelf;
            dictionaryPanel.SetActive(!isActive);
        }
    }
}
