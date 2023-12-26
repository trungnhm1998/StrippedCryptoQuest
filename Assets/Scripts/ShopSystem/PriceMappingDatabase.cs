using System;
using System.Collections.Generic;
using CryptoQuest.Item.Consumable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.ShopSystem
{
    public class PriceMappingDatabase : ScriptableObject
    {
        [Serializable]
        public struct PriceConfig
        {
            public string ItemId;
            public float BuyingPrice;
            public float SellingPrice;
        }

        [SerializeField] private PriceConfig[] _itemPrices;
        
        private readonly Dictionary<string, PriceConfig> _priceMapping = new();

        private void OnEnable()
        {
            foreach (var priceConfig in _itemPrices)
            {
                _priceMapping.Add(priceConfig.ItemId, priceConfig);
            }
        }

        public bool TryGetSellingPrice(string itemId, out float price)
        {
            if (_priceMapping.TryGetValue(itemId, out var priceConfig))
            {
                price = priceConfig.SellingPrice;
                return true;
            }

            price = 0;
            return false;
        }
        
        public bool TryGetBuyingPrice(string itemId, out float price)
        {
            if (_priceMapping.TryGetValue(itemId, out var priceConfig))
            {
                price = priceConfig.BuyingPrice;
                return true;
            }

            price = 0;
            return false;
        }
    }
}