using TMPro;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TMP_Text coinText;

    void OnEnable()
    {
        StartCoroutine(ConnectAfterFrame());
    }

    void Start()
    {
        TryConnect();
        StartCoroutine(DelayedConnect());
    }

    IEnumerator ConnectAfterFrame()
    {
        yield return null;
        TryConnect();
    }

    void TryConnect()
    {
        if (CoinManager.Instance != null && coinText != null)
            CoinManager.Instance.SetCoinUI(coinText);
    }

    IEnumerator DelayedConnect()
    {
        yield return new WaitForSeconds(0.1f);
        if (coinText != null && CoinManager.Instance != null &&
            (CoinManager.Instance.coinText == null || CoinManager.Instance.coinText != coinText))
        {
            TryConnect();
        }
    }
}