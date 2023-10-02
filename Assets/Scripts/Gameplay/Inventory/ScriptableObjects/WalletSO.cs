using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletSO : ScriptableObject
    {
        public CurrencyInfo Gold = new();
        public CurrencyInfo Soul = new();
        public CurrencyInfo Diamond = new();
        public Dictionary<CurrencySO, CurrencyInfo> CurrencyAmounts = new();

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
#if UNITY_EDITOR
            if (Gold.Data == null || Soul.Data == null || Diamond.Data == null)
                return;
#endif
            CurrencyAmounts[Gold.Data] = Gold;
            CurrencyAmounts[Diamond.Data] = Diamond;
            CurrencyAmounts[Soul.Data] = Soul;
            ValidateAmount();
        }

        private void ValidateAmount()
        {
            Gold.ValidateAmount();
            Soul.ValidateAmount();
            Diamond.ValidateAmount();
        }
    }
}