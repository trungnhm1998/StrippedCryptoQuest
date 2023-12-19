using CryptoQuest.Inventory.Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Commons.UI
{
    public class UICurrencyText : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _currencyText;
        [SerializeField] protected Image _currencyIcon;
        [SerializeField] protected string _currencyTextFormat;
        [SerializeField] protected CurrencySO _currencySO;

        private void Start()
        {
            if (_currencyIcon.sprite == null)
                LoadCurrencyIcon();
        }

        private void LoadCurrencyIcon()
        {
            if (_currencySO.Image == null) return;
            _currencySO.Image.LoadAssetAsync<Sprite>().Completed += handle =>
            {
                _currencyIcon.sprite = handle.Result;
            };
        }

        public virtual void SetValue(float amount)
        {
            _currencyText.text = string.Format(_currencyTextFormat, amount);
        }
    }
}
