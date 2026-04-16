using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [Header("Visual Components")]
    public SpriteRenderer playerSpriteRenderer; // I-drag dito yung "Sprite" child object sa Inspector

    void Awake()
    {
        // Kung hindi mo nai-drag sa inspector, hahanapin ito ng script sa mga "anak" na object
        if (playerSpriteRenderer == null)
        {
            playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void ApplyAppearance(PlayerData data)
    {
        if (data == null) return;

        // Siguraduhin na nahanap ang SpriteRenderer bago mag-apply
        if (playerSpriteRenderer == null)
        {
            playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (playerSpriteRenderer != null)
        {
            // DITO MO ILALAGAY YUNG LOGIC NG PAGPALIT NG SPRITE
            // Halimbawa, base sa gender o ID na nasa PlayerData
            // playerSpriteRenderer.sprite = data.selectedSprite; 

            // --- CRITICAL VISIBILITY FIXES ---

            // 1. Force Sorting Layer (Siguraduhing may "Player" layer ka sa Tag & Layers)
            playerSpriteRenderer.sortingLayerName = "Player";
            playerSpriteRenderer.sortingOrder = 15; // Mataas na number para laging nasa harap

            // 2. Force Color Alpha to 1 (Baka naka-0 kaya invisible)
            Color c = playerSpriteRenderer.color;
            c.a = 1f;
            playerSpriteRenderer.color = c;

            Debug.Log($"PlayerSetup: Appearance applied for {data.PlayerName}");
        }
        else
        {
            Debug.LogError("PlayerSetup: SpriteRenderer not found on Player or its children!");
        }
    }

    // Opsyonal: Gamitin ito para ma-check sa Scene view kung nasaan ang player
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}