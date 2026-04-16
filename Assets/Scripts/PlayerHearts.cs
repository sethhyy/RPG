using UnityEngine;

public class PlayerHearts : MonoBehaviour
{
    public GameObject[] hearts; // Drag Heart1 → Heart5 here in Inspector

    void Update()
    {
        int currentHearts = GameManager.Instance.playerData.CurrentHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
            // Show heart if i < currentHearts, hide otherwise
            hearts[i].SetActive(i < currentHearts);
        }
    }
}
