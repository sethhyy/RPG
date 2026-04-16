using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PanelLink
{
    public Button button;
    public GameObject panelToClose;
    public GameObject panelToOpen;
}

public class PanelManager : MonoBehaviour
{
    [Header("Navigation Configuration")]
    public PanelLink[] navigationLinks;

    private bool isInitialized = false;

    void Start()
    {
        StartCoroutine(InitializeNavigation());
    }

    void OnEnable()
    {
        if (isInitialized)
        {
            StartCoroutine(InitializeNavigation());
        }
    }

    void OnDisable()
    {
        CleanupListeners();
    }

    void OnDestroy()
    {
        CleanupListeners();
    }

    IEnumerator InitializeNavigation()
    {
        yield return new WaitForEndOfFrame();

        if (navigationLinks == null || navigationLinks.Length == 0)
        {
            yield break;
        }

        CleanupListeners();

        for (int i = 0; i < navigationLinks.Length; i++)
        {
            PanelLink link = navigationLinks[i];

            if (link.button == null || link.panelToClose == null || link.panelToOpen == null)
            {
                continue;
            }

            int index = i;
            link.button.onClick.AddListener(() => ExecuteTransition(index));
        }

        isInitialized = true;
    }

    void ExecuteTransition(int index)
    {
        if (index < 0 || index >= navigationLinks.Length) return;

        PanelLink link = navigationLinks[index];

        if (link.panelToClose != null)
            link.panelToClose.SetActive(false);

        if (link.panelToOpen != null)
            link.panelToOpen.SetActive(true);
    }

    void CleanupListeners()
    {
        if (navigationLinks == null) return;

        foreach (PanelLink link in navigationLinks)
        {
            if (link.button != null)
            {
                link.button.onClick.RemoveAllListeners();
            }
        }
    }
}