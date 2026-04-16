using UnityEngine;

public class PlayerLockController : MonoBehaviour
{
    private MonoBehaviour[] movementScripts;

    private void Awake()
    {
        // Get all movement-related scripts on the player
        movementScripts = GetComponents<MonoBehaviour>();
    }

    public void LockPlayer()
    {
        foreach (var script in movementScripts)
        {
            if (script != this)
                script.enabled = false;
        }
    }

    public void UnlockPlayer()
    {
        foreach (var script in movementScripts)
        {
            script.enabled = true;
        }
    }
}
