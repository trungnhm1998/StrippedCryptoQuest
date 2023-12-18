using System;
using CryptoQuest.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Currency
{
    [Serializable]
    public class CurrencyInfo : ItemInfo<CurrencySO>
    {
        public event Action<CurrencyInfo> AmountChanged;
        [SerializeField] private float _amount;

        public float Amount
        {
            get => _amount;
            set
            {
                _amount = Mathf.Max(0, value);
                AmountChanged?.Invoke(this);
            }
        }

        public CurrencyInfo() { }

        public CurrencyInfo(CurrencySO item, float amount) : base(item)
        {
            Amount = amount;
        }

        public void SetAmount(float amount)
        {
            Amount = amount;
            AmountChanged?.Invoke(this);
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
                AmountChanged?.Invoke(this);
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
    }
}