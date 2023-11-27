using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStonePriceMappingSO : ScriptableObject
    {
        public List<MagicStonePriceMapping> MagicStonePriceMappings = new();
        private Dictionary<string, MagicStonePriceMapping> _magicStonePriceMappingDictionary = new();

        public Dictionary<string, MagicStonePriceMapping> MagicStonePriceMappingDictionary =>
            _magicStonePriceMappingDictionary;

        private void OnEnable()
        {
            foreach (var mapping in MagicStonePriceMappings)
            {
                _magicStonePriceMappingDictionary[mapping.MagicStoneId] = mapping;
            }
        }
    }

    [Serializable]
    public class MagicStonePriceMapping
    {
        public string MagicStoneId;
        public int Price;
        public int SellingPrice;
    }
}