using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class CurrencyPresenter : MonoBehaviour
    {
        [SerializeField] private UIBlackSmithCurrency _currencyUI;
        public event UnityAction OnSendSuccess;
        public event UnityAction OnSendFailed;
        private ICurrenciesController _currenciesController;
        public float Gold { get; private set; }
        public float Diamond { get; private set; }

        private void OnEnable()
        {
            _currenciesController = ServiceProvider.GetService<ICurrenciesController>();
            UpdateCurrenciesUI();
        }

        private void UpdateCurrenciesUI()
        {
            Gold = _currenciesController.Wallet.Gold.Amount;
            Diamond = _currenciesController.Wallet.Diamond.Amount;
            _currencyUI.UpdateCurrency(Gold, Diamond);
        }

        public void CurrencyNeeded(float quantityGold, float quantityDiamond)
        {
            var goldSO = _currenciesController.Wallet.Gold;
            var diamondSO = _currenciesController.Wallet.Diamond;
            if (!goldSO.CanUpdateAmount(-quantityGold)
                          || !diamondSO.CanUpdateAmount(-quantityDiamond))
            {
                OnSendFailed.Invoke();
                return;
            }

            _currenciesController.UpdateCurrencyAmount(goldSO.Data, -quantityGold);
            _currenciesController.UpdateCurrencyAmount(diamondSO.Data, -quantityGold);
            UpdateCurrenciesUI();
            OnSendSuccess.Invoke();
        }
    }
}