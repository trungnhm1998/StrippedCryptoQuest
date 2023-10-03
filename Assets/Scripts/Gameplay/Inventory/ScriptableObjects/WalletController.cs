using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletController : MonoBehaviour, ICurrenciesController
    {
        [SerializeField] private WalletControllerSO _walletController;
        [field: SerializeField] public WalletSO Wallet { get; set; }

        public void Awake()
        {
            _walletController.Provide(this);
            ServiceProvider.Provide<ICurrenciesController>(this);
        }

        public void GetCurrencyInfo(CurrencySO currencySo, out CurrencyInfo currencyInfo)
        {
            Wallet.TryGetValue(currencySo, out currencyInfo);
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
            Wallet = walletSo;
        }
#endif
    }
}