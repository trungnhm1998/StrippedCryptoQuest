using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop
{
    public class ItemPriceMappingDatabase : ScriptableObject
    {
        [Serializable]
        public class PriceMapping
        {
            public string ItemId;
            public float BuyingPrice;
            public float SellingPrice;
        }

        [SerializeField] private PriceMapping[] _priceMappings;

        private Dictionary<string, PriceMapping> _priceMappingsLookupTable = default;

        private void OnEnable()
        {
            _priceMappingsLookupTable = new Dictionary<string, PriceMapping>();
            foreach (var priceMapping in _priceMappings)
                _priceMappingsLookupTable.Add(priceMapping.ItemId, priceMapping);
        }

        public bool TryGetItemMapping(string equipmentId, out PriceMapping mapping) =>
            _priceMappingsLookupTable.TryGetValue(equipmentId, out mapping);

        public PriceMapping this[string equipmentId] => _priceMappingsLookupTable[equipmentId];
    }
}