using CryptoQuest.Events;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    public class TreasureController : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _provider;
        [SerializeField] private TreasureDatabase _lootDatabase;

        [Header("Listen to Events")]
        [SerializeField] private StringEventChannelSO _requestGetLootEvent;

        private void OnEnable()
        {
            _requestGetLootEvent.EventRaised += RequestGetLoot;
        }

        private void OnDisable()
        {
            _requestGetLootEvent.EventRaised -= RequestGetLoot;
        }

        private void RequestGetLoot(string lootId)
        {
            if (!string.IsNullOrEmpty(lootId))
                HandleGetLoot(lootId);
        }

        private void HandleGetLoot(string lootId)
        {
            var isLootExist = _lootDatabase.LootDict.TryGetValue(lootId, out LootTable lootData);
            if (!isLootExist)
            {
                Debug.LogWarning($"Loot with id {lootId} does not exist in database!");
                return;
            }

            AddLootsToInventory(lootData);
        }

        private void AddLootsToInventory(LootTable lootInfo)
        {
            foreach (var loot in lootInfo.LootInfos)
            {
                loot.AddItemToInventory(_provider.Inventory);
            }
        }
    }
}