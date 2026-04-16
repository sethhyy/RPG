using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public string[] npcLines;
    public GameObject dialogueCanvas;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;

    private int currentLine = 0;

    void Start()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    public void Interact()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(true);
            currentLine = 0;
            ShowLine();
        }
    }

    void ShowLine()
    {
        if (currentLine < npcLines.Length)
        {
            dialogueText.text = npcLines[currentLine];
            for (int i = 0; i < choiceButtons.Length; i++)
                choiceButtons[i].gameObject.SetActive(false); // hide choices by default
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void EndDialogue()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }
}
