using UnityEngine;

public class NPCQuizTrigger2D : MonoBehaviour
{
    public QuizManager quizManager;
    public bool isTutorialNPC = false;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        if (isTutorialNPC)
        {
            quizManager.TriggerTutorialQuiz();   // Tatay (tutorial)
        }
        else
        {
            quizManager.TriggerQuizWithGuide();  // Nanay / normal NPC
        }
    }
}
