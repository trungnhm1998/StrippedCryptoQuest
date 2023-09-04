using System;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CryptoQuest.Gameplay.Inventory.Currency
{
    public class CurrencyPrototype : MonoBehaviour
    {
        public WalletSO Wallet;
        public Text GoldTxt;
        public Text DiamondTxt;
        public Text SoulTxt;
        public InputField Amount;
        [SerializeField] private WalletControllerSO _walletControllerSo;

        private ICurrenciesController _walletController;

        private void Awake()
        {
            _walletController = _walletControllerSo;
            UpdateUI();
        }

        public void AddCurrencyAmount(CurrencySO currencySo)
        {
            _walletController.UpdateCurrencyAmount(currencySo, Amount.text == "" ? 0 : float.Parse(Amount.text));
            UpdateUI();
        }

        public void RemoveCurrencyAmount(CurrencySO currencySo)
        {
            _walletController.UpdateCurrencyAmount(currencySo, Amount.text == "" ? 0 : -float.Parse(Amount.text));
            UpdateUI();
        }

        public void UpdateUI()
        {
            GoldTxt.text = $"Gold  {Wallet.Gold.Amount}";
            DiamondTxt.text = $"Diamond  {Wallet.Diamond.Amount}";
            SoulTxt.text = $"Soul  {Wallet.Soul.Amount}";
        }
    }
}