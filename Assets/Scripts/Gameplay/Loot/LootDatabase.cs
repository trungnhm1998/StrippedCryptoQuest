using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Loot
{
    public class LootDatabase : ScriptableObject
    {
        [SerializeField] private List<AssetReferenceT<LootTable>> _loots;

        // TODO: Unload the asset when not needed or when the new scene is loaded
        private readonly Dictionary<int, LootTable> _loadedLoot = new Dictionary<int, LootTable>();

        public IEnumerator LoadLoots(int id)
        {
            if (_loadedLoot.ContainsKey(id))
                yield break;
            if (id < 0 || id >= _loots.Count)
            {
                Debug.LogWarning($"Loot id {id} is out of range");
                yield break;
            }

            var lootAsset = _loots[id];
            var handle = lootAsset.LoadAssetAsync();
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"Failed to load loot {lootAsset} at id {id}");
                yield break;
            }

            var loot = handle.Result;
            if (loot == null)
            {
                Debug.LogWarning($"Failed to load loot {lootAsset} at id {id}");
                yield break;
            }

            _loadedLoot.Add(id, loot);
        }

        public LootTable GetLoots(int lootTableId)
        {
            if (_loadedLoot.TryGetValue(lootTableId, out var loot))
                return loot;
            Debug.LogWarning($"Loot table id {lootTableId} not found");
            return CreateInstance<LootTable>();
        }
    }
}