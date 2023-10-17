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
        [SerializeField] private UIBlackSmithCurrency _currency;
        public event UnityAction OnSendSuccess;
        public event UnityAction OnSendFailed;
        private ICurrenciesController _currenciesController;
        public float Gold { get; private set; }
        private float _diamond;

        private void OnEnable()
        {
            _currenciesController = ServiceProvider.GetService<ICurrenciesController>();
            GetCurrencies();
        }

        private void GetCurrencies()
        {
            Gold = _currenciesController.Wallet.Gold.Amount;
            _diamond = _currenciesController.Wallet.Diamond.Amount;
            _currency.UpdateCurrency(Gold, _diamond);
        }

        public void CurrencyNeeded(float quantityGold, float quantityDiamond)
        {
            if (Gold >= quantityGold && _diamond >= quantityDiamond)
            {
                _currenciesController.Wallet.Gold.SetCurrencyAmount(Gold - quantityGold);
                _currenciesController.Wallet.Diamond.SetCurrencyAmount(_diamond - quantityDiamond);
                GetCurrencies();
                OnSendSuccess.Invoke();
            }
            else
                OnSendFailed.Invoke();
        }
    }
}