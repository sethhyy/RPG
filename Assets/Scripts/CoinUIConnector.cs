using UnityEngine;
using TMPro;

public class CoinUIConnector : MonoBehaviour
{
    private TMP_Text _coinText;

    void OnEnable()
    {
        Register();
    }

    void Start()
    {
        Register();
    }

    void Register()
    {
        if (_coinText == null)
            _coinText = GetComponent<TMP_Text>();

        if (CoinManager.Instance != null && _coinText != null)
            CoinManager.Instance.SetCoinUI(_coinText);
    }
}