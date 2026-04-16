using UnityEngine;

public class FloatingGuideButton : MonoBehaviour
{
    public GameObject guidePanel; // assign the guide panel image

    private void Start()
    {
        if (guidePanel != null)
            guidePanel.SetActive(false);
    }

    // Call this when the button is clicked
    public void OpenGuide()
    {
        if (guidePanel != null)
            guidePanel.SetActive(true);
    }

    // Close button inside guide panel
    public void CloseGuide()
    {
        if (guidePanel != null)
            guidePanel.SetActive(false);
    }
}
