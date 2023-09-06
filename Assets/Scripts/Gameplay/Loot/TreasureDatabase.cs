using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    public class TreasureDatabase : ScriptableObject
    {
        [SerializeField] private List<LootTable> _loots;

        private Dictionary<string, LootTable> _lootDict = new();

        //TODO: turn LootData to AssetReferenceT<LootData> and use Addressable to load it
        //TODO: cache loaded loot data
        public Dictionary<string, LootTable> LootDict
        {
            get
            {
                if (_lootDict != null && _lootDict.Count > 0)
                    return _lootDict;

                _lootDict = _loots.ToDictionary(data => data.ID);

                return _lootDict;
            }
        }
    }
}