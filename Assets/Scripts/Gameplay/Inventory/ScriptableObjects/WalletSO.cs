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

        public void OnValidate()
        {
            if (Gold.Data == null || Soul.Data == null || Diamond.Data == null)
                return;
            ValidateAmount();
            CurrencyAmounts[Gold.Data] = Gold;
            CurrencyAmounts[Diamond.Data] = Diamond;
            CurrencyAmounts[Soul.Data] = Soul;
        }

        private void ValidateAmount()
        {
            Gold.ValidateAmount();
            Soul.ValidateAmount();
            Diamond.ValidateAmount();
        }
    }
}