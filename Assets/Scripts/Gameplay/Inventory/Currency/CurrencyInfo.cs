using System;
using CryptoQuest.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Currency
{
    [Serializable]
    public class CurrencyInfo : ItemInfo<CurrencySO>
    {
        [field: SerializeField] public float Amount { get; private set; } = 0;
        public override int Price => 0;
        public override int SellPrice => 0;

        public CurrencyInfo() { }

        public CurrencyInfo(CurrencySO item, float amount) : base(item)
        {
            Amount = amount;
        }

        public void SetCurrencyAmount(float amount)
        {
            Amount = amount;
        }

        public bool CanUpdateAmount(float amount)
        {
            return Amount + amount >= 0;
        }

        public void UpdateCurrencyAmount(float amount)
        {
            if (CanUpdateAmount(amount))
            {
                Amount += amount;
            }
            else
            {
                Debug.LogWarning("Insufficient funds!");
            }
        }

        public void ValidateAmount()
        {
            if (Amount < 0)
            {
                Amount = 0;
            }
        }

        public CurrencyInfo Clone()
        {
            return new CurrencyInfo(Data, Amount);
        }
    }
}