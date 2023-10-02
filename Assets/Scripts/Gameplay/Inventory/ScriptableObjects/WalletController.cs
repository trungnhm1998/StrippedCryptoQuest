using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletController : MonoBehaviour, ICurrenciesController
    {
        [SerializeField] private WalletControllerSO _walletController;
        [SerializeField] private WalletSO _wallet;

        public void Awake()
        {
            _walletController.Provide(this);
        }

        public void GetCurrencyInfo(CurrencySO currencySo, out CurrencyInfo currencyInfo)
        {
            _wallet.CurrencyAmounts.TryGetValue(currencySo, out currencyInfo);
        }

        public void UpdateCurrencyAmount(CurrencySO currencySo, float amount)
        {
            GetCurrencyInfo(currencySo, out var currencyInfo);
            currencyInfo.UpdateCurrencyAmount(amount);
        }
#if UNITY_EDITOR
        public void Editor_SetWalletController(WalletControllerSO walletControllerSo)
        {
            _walletController = walletControllerSo;
        }
        
        public void Editor_SetWallet(WalletSO walletSo)
        {
            _wallet = walletSo;
        }
#endif
    }
}