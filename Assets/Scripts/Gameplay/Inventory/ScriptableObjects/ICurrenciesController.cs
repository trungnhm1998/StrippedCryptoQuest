using CryptoQuest.Gameplay.Inventory.Currency;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public interface ICurrenciesController
    {
        public WalletSO Wallet { get; set; }
        void GetCurrencyInfo(CurrencySO currencySo, out CurrencyInfo currencyInfo);
        void UpdateCurrencyAmount(CurrencySO currencySo, float amount);
    }
}