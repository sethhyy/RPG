using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string targetMapName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. LOGIC: Save the current state BEFORE leaving
            if (FirebaseManager.Instance != null)
            {
                FirebaseManager.Instance.SaveCurrentProgress();
            }

            // 2. UX: Load the next area
            SceneManager.LoadScene(targetMapName);
        }
    }
}