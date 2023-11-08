using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIMetadSourceButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _currency;
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Text _amount;
        private CurrencyInfo _currencyInfo;
        public CurrencyInfo Currency => _currencyInfo;

        private void Start()
        {
            _currencyInfo = _wallet[_currency];
        }

        private void OnEnable()
        {
            _currencyInfo.AmountChanged += UpdateAmount;
            UpdateAmount(_currencyInfo);
        }

        private void OnDisable()
        {
            _currencyInfo.AmountChanged -= UpdateAmount;
        }

        private void UpdateAmount(CurrencyInfo currency) => _amount.text = currency.Amount.ToString();

        public void OnSelect(BaseEventData eventData)
        {
            _arrow.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _arrow.SetActive(false);
        }
    }
}