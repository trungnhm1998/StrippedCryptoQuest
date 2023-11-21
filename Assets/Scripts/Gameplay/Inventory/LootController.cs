using CryptoQuest.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class LootController : MonoBehaviour
    {
        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IInventoryController _inventory;

        private void Awake() => _inventory ??= ServiceProvider.GetService<IInventoryController>();

        protected void OnEnable() => _addLootRequestEventChannel.EventRaised += AddLoot;

        protected void OnDisable() => _addLootRequestEventChannel.EventRaised -= AddLoot;

        private void AddLoot(LootInfo loot) => loot.AddItemToInventory(_inventory);
    }
}