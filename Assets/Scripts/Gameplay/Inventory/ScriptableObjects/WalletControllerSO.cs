using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletControllerSO : ScriptableObject, ICurrenciesController
    {
        public WalletSO Wallet;

        public void GetCurrencyInfo(CurrencySO currencySo, out CurrencyInfo currencyInfo)
        {
            Wallet.CurrencyAmounts.TryGetValue(currencySo, out currencyInfo);
        }

        public void UpdateCurrencyAmount(CurrencySO currencySo, float amount)
        {
            GetCurrencyInfo(currencySo, out var currencyInfo);
            currencyInfo.UpdateCurrencyAmount(amount);
        }
    }
}