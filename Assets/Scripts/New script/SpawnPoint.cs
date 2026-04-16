using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Unique ID for this spawn point")]
    public string spawnID;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }

}
