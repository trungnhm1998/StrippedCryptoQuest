using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletControllerSO : ScriptableObject, ICurrenciesController
    {
        private ICurrenciesController _walletController;
        public void Provide(ICurrenciesController walletController) => _walletController = walletController;

        public void GetCurrencyInfo(CurrencySO currencySo, out CurrencyInfo currencyInfo)
        {
            _walletController.GetCurrencyInfo(currencySo, out currencyInfo);
        }

        public void UpdateCurrencyAmount(CurrencySO currencySo, float amount)
        {
            _walletController.UpdateCurrencyAmount(currencySo, amount);
        }
    }
}