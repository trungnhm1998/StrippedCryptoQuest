using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Inn.UI;
using UnityEngine;

namespace CryptoQuest.Inn
{
    public class InnController : MonoBehaviour
    {
        [field: Header("Wallet Settings")]
        [field: SerializeField] public UICurrencyPanel CurrencyPanel { get; private set; }

        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;

        [Header("Components")]
        [SerializeField] private RestorationController _restorationController;

        [SerializeField] private InnPresenter _innPresenter;
        private float _currentGold => _wallet[_gold].Amount;
        private float _innCost => _innPresenter.InnPrice;

        public void ShowCurrency()
        {
            CurrencyPanel.ShowCurrency();
            UpdateCurrency();
        }
        
        public void HideCurrency() => CurrencyPanel.HideCurrency();

        public void RestoreParty()
        {
            if (!_restorationController.RestoreParty())
            {
                Debug.LogWarning($"Party is not valid or not alive");
                return;
            }

            ReduceGold();
            UpdateCurrency();
        }

        private void ReduceGold() => _wallet[_gold].SetAmount(_currentGold - _innCost);
        private void UpdateCurrency() => CurrencyPanel.UpdateCurrency(_currentGold);
    }
}