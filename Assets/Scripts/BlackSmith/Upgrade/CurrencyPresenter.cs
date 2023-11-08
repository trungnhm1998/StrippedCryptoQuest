using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class CurrencyPresenter : MonoBehaviour
    {
        [SerializeField] private UIBlackSmithCurrency _currencyUI;
        public event UnityAction OnSendSuccess;
        public float Gold { get; private set; }
        public float Diamond { get; private set; }
        private WalletSO _wallet;
        private float _quantityGold, _quantityDiamond;

        private void OnEnable()
        {
            // _currenciesController = ServiceProvider.GetService<ICurrenciesController>();
            // _wallet = _currenciesController.Wallet;
            UpdateCurrenciesUI();
        }

        private void UpdateCurrenciesUI()
        {
            // Gold = _wallet.Gold.Amount;
            // Diamond = _wallet.Diamond.Amount;
            _currencyUI.UpdateCurrency(Gold, Diamond);
        }

        public void CurrencyNeeded(float gold, float diamond)
        {
            _quantityGold = gold;
            _quantityDiamond = diamond;
            // if (!_wallet.Gold.CanUpdateAmount(-_quantityGold) ||
            //     !_wallet.Diamond.CanUpdateAmount(-_quantityDiamond)) return;
            OnSendSuccess.Invoke();
        }

        public void UpdateCurrencyAmount()
        {
            // _currenciesController.Wallet.Gold.UpdateCurrencyAmount(-_quantityGold);
            // _currenciesController.Wallet.Diamond.UpdateCurrencyAmount(-_quantityDiamond);
            UpdateCurrenciesUI();
        }
    }
}