using UnityEngine;
using TMPro;

public class QuestMessageUI : MonoBehaviour
{
    public TMP_Text messageText;

    void Start()
    {
        int progress = PlayerPrefs.GetInt("QuestProgress", 0);

        if (progress <= 0)
        {
            messageText.text =
                "Unang alintuntunin:\nHanapin si Tatay sa loob ng bahay.";
        }
        else if (progress == 1)
        {
            messageText.text =
                "Magaling!\nTulungan mo naman si Inay sa loob ng bahay.";
        }
        else if (progress == 2)
        {
            messageText.text =
                "Ayos!\nTumuloy ka na sa susunod na antas:\nPALENGKE.";
        }
    }
}
