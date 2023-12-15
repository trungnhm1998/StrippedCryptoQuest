using System;
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
            public float SellingPrice;
            public float BuyingPrice;
        }

        [SerializeField] private PriceConfig[] _itemPrices;

        public float GetPrice(ConsumableInfo consumable)
        {
            return Random.Range(10, 150);
        }

        public float GetPrice(string equipmentPrefabId)
        {
            return Random.Range(100, 1000);
        }
    }
}