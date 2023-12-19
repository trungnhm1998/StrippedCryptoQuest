using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UICurrency : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _currency;
        public CurrencySO Currency => _currency;

        [Header("UI Components")]
        [SerializeField] private Text _text;

        private void OnEnable()
        {
            _wallet[_currency].AmountChanged += UpdateAmount;
            UpdateAmount(_wallet[_currency]);
        }

        private void OnDisable()
        {
            _wallet[_currency].AmountChanged -= UpdateAmount;
        }

        private void UpdateAmount(CurrencyInfo currency) => _text.text = currency.Amount.ToString();
    }
}