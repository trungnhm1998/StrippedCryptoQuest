using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class CurrencyPresenter : MonoBehaviour
    {
        public event UnityAction UpdateCurrenciesSucceed;

        [SerializeField] private WalletSO _walletSO;
        [SerializeField] private CurrencySO _goldSO;
        [SerializeField] private CurrencySO _diamondSO;
        [SerializeField] private UIBlackSmithCurrency _currencyUI;
        public float Gold { get; private set; }
        public float Diamond { get; private set; }
        private WalletSO _wallet;
        private float _quantityGold, _quantityDiamond;

        private void OnEnable()
        {
            UpdateCurrenciesUI();
        }

        private void UpdateCurrenciesUI()
        {
            Gold = _walletSO[_goldSO].Amount;
            Diamond = _walletSO[_diamondSO].Amount;
            _currencyUI.UpdateCurrency(Gold, Diamond);
        }

        public void RequestUpdateCurrencies(float gold, float diamond)
        {
            _quantityGold = gold;
            _quantityDiamond = diamond;
            if (!_walletSO[_goldSO].CanUpdateAmount(-_quantityGold) ||
                !_walletSO[_diamondSO].CanUpdateAmount(-_quantityDiamond)) return;
                
            UpdateCurrenciesAmount();
            UpdateCurrenciesUI();
            UpdateCurrenciesSucceed?.Invoke();
        }

        private void UpdateCurrenciesAmount()
        {
            _walletSO[_goldSO].UpdateCurrencyAmount(-_quantityGold);
            _walletSO[_diamondSO].UpdateCurrencyAmount(-_quantityDiamond);
        }
    }
}