using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Currency;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public class WalletSO : ScriptableObject
    {
        [field: SerializeField] public CurrencyInfo[] Currencies { get; private set; } = Array.Empty<CurrencyInfo>();

        private Dictionary<CurrencySO, CurrencyInfo> _currencies;

        private Dictionary<CurrencySO, CurrencyInfo> CurrenciesDictionary
        {
            get
            {
                if (_currencies != null) return _currencies;

                _currencies = new Dictionary<CurrencySO, CurrencyInfo>();
                foreach (var currency in Currencies)
                {
                    CurrenciesDictionary.Add(currency.Data, currency);
                }

                return _currencies;
            }
        }

        public CurrencyInfo this[CurrencySO currency] => CurrenciesDictionary[currency];
    }
}