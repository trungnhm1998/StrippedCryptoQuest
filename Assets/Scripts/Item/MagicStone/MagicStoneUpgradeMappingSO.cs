using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneUpgradeMappingSO : ScriptableObject
    {
        [field: SerializeField] public List<MagicStoneUpgradeMapping> MagicStoneUpgradeMappings = new();
        private Dictionary<string, string> _magicStoneUpgradeMappingDictionary = new();

        public Dictionary<string, string> MagicStoneUpgradeMappingDictionary =>
            _magicStoneUpgradeMappingDictionary;

        private void OnEnable()
        {
            foreach (var mapping in MagicStoneUpgradeMappings)
            {
                _magicStoneUpgradeMappingDictionary[mapping.MagicStoneId] = mapping.UpgradedMagicStoneId;
            }
        }
    }

    [Serializable]
    public class MagicStoneUpgradeMapping
    {
        public string MagicStoneId;
        public string UpgradedMagicStoneId;
    }
}