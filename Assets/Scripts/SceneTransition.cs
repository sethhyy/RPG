using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Transition")]
    public string targetScene;

    [Header("Spawn Settings")]
    public string spawnIDInTargetScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        SpawnManager.NextSpawnID = spawnIDInTargetScene;
        SceneManager.LoadScene(targetScene);
    }
}
