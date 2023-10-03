using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletSO : ScriptableObject
    {
        public CurrencyInfo Gold = new();
        public CurrencyInfo Soul = new();
        public CurrencyInfo Diamond = new();
        private readonly Dictionary<CurrencySO, CurrencyInfo> _currencyAmounts = new();

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (Gold.Data == null || Soul.Data == null || Diamond.Data == null)
                return;
            ValidateAmount();
        }

        public void Editor_Enable()
        {
            OnEnable();
        }
#endif

        private void OnEnable()
        {
            if (Gold.Data)
                _currencyAmounts[Gold.Data] = Gold;
            if (Diamond.Data)
                _currencyAmounts[Diamond.Data] = Diamond;
            if (Soul.Data)
                _currencyAmounts[Soul.Data] = Soul;
            ValidateAmount();
        }

        public bool TryGetValue(CurrencySO currencySo, out CurrencyInfo currencyInfo)
            => _currencyAmounts.TryGetValue(currencySo, out currencyInfo);

        private void ValidateAmount()
        {
            Gold.ValidateAmount();
            Soul.ValidateAmount();
            Diamond.ValidateAmount();
        }
    }
}