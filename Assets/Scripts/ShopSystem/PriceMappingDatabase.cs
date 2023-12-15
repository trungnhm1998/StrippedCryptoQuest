using System;
using UnityEngine;

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
    }
}