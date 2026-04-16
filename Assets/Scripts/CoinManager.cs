using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public TMP_Text coinText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        StartCoroutine(RebindCoinUIAfterFrame());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        coinText = null;
        StartCoroutine(RebindCoinUIAfterFrame());
    }

    IEnumerator RebindCoinUIAfterFrame()
    {
        yield return null;
        yield return null;
        TryAutoBindCoinText();
        UpdateUI();
    }

    void TryAutoBindCoinText()
    {
        foreach (var ui in FindObjectsByType<UIManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (ui.coinText != null)
            {
                SetCoinUI(ui.coinText);
                return;
            }
        }

        foreach (var connector in FindObjectsByType<CoinUIConnector>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            var tmp = connector.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                SetCoinUI(tmp);
                return;
            }
        }

        var texts = FindObjectsByType<TMP_Text>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var t in texts)
        {
            if (t.gameObject.name.Equals("coins", StringComparison.OrdinalIgnoreCase))
            {
                SetCoinUI(t);
                return;
            }
        }

        foreach (var t in texts)
        {
            if (t.gameObject.name.IndexOf("coin", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                SetCoinUI(t);
                return;
            }
        }
    }

    public void SetCoinUI(TMP_Text newText)
    {
        coinText = newText;
        FixCoinsRowIfNeeded(coinText);
        UpdateUI();
        Canvas.ForceUpdateCanvases();
    }

    /// <summary>
    /// "coins" uses TMP on the root plus a child coin Image. Huge font sizes or icon overlap hide the count (e.g. Palengke).
    /// </summary>
    static void FixCoinsRowIfNeeded(TMP_Text tmp)
    {
        if (tmp == null) return;

        Transform iconT = FindCoinIconTransform(tmp);
        if (iconT == null) return;

        if (iconT is RectTransform irt)
        {
            irt.SetAsFirstSibling();
            irt.anchorMin = new Vector2(1f, 0.5f);
            irt.anchorMax = new Vector2(1f, 0.5f);
            irt.pivot = new Vector2(1f, 0.5f);
            irt.anchoredPosition = new Vector2(-8f, 0f);
            irt.sizeDelta = new Vector2(72f, 72f);
        }

        tmp.margin = new Vector4(8f, 4f, 86f, 4f);
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 22f;
        tmp.fontSizeMax = 54f;
        tmp.overflowMode = TextOverflowModes.Overflow;
        tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
        tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    static Transform FindCoinIconTransform(TMP_Text tmp)
    {
        var t = tmp.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            var c = t.GetChild(i);
            if (c.name.IndexOf("coinIcon", StringComparison.OrdinalIgnoreCase) >= 0)
                return c;
        }

        var p = t.parent;
        if (p != null)
        {
            for (int i = 0; i < p.childCount; i++)
            {
                var c = p.GetChild(i);
                if (c != t && c.name.IndexOf("coinIcon", StringComparison.OrdinalIgnoreCase) >= 0)
                    return c;
            }
        }

        return null;
    }

    public void AddCoin(int amount = 1)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerData.gold += amount;
            UpdateUI();

            if (FirebaseManager.Instance != null)
                FirebaseManager.Instance.SaveCurrentProgress();
        }
    }

    public void UpdateUI()
    {
        if (coinText != null && GameManager.Instance != null)
        {
            coinText.text = GameManager.Instance.playerData.gold.ToString();
            coinText.ForceMeshUpdate(true);
        }
    }
}
