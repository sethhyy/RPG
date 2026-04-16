using UnityEngine;
using TMPro; // Kung TextMeshPro ang ginagamit

public class DictionaryUI : MonoBehaviour
{
    public GameObject dictionaryPanel; // Reference sa panel
    public TextMeshProUGUI wordText;   // Reference sa Word text
    public TextMeshProUGUI meaningText; // Reference sa Meaning text

    // Optionally, maglagay ng dynamic words
    [System.Serializable]
    public struct WordEntry
    {
        public string word;
        public string meaning;
    }

    public WordEntry[] words;

    public void OpenDictionary()
    {
        if (words.Length > 0)
        {
            // Example: show first word
            wordText.text = "Salita: " + words[0].word;
            meaningText.text = "Kahulugan: " + words[0].meaning;
        }
        dictionaryPanel.SetActive(true);
    }

    public void CloseDictionary()
    {
        dictionaryPanel.SetActive(false);
    }
}
