using UnityEngine;
using UnityEngine.UI;

public class UIHeartManager : MonoBehaviour
{
    void Start()
    {
        Image[] heartsInScene = new Image[5];

        for (int i = 0; i < 5; i++)
        {
            GameObject heartObj = GameObject.Find("Heart" + (i + 1));
            if (heartObj != null)
            {
                heartsInScene[i] = heartObj.GetComponent<Image>();
            }
        }

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.SetHeartUI(heartsInScene);
        }
    }
}