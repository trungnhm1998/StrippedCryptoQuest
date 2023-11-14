using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        InventorySO Inventory { get; }
        bool Add(EquipmentInfo equipment);
        bool Remove(EquipmentInfo equipment);
        bool Add(NftEquipment equipment);
        bool Remove(NftEquipment equipment);
        bool Add(ConsumableInfo consumable);
        bool Remove(ConsumableInfo consumable);
        bool Contains(EquipmentInfo equipment);
        bool Contains(NftEquipment equipment);
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private void Awake()
        {
            ServiceProvider.Provide<IInventoryController>(this);
        }

        protected void OnEnable()
        {
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        protected void OnDisable()
        {
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void AddLoot(LootInfo loot)
        {
            loot.AddItemToInventory(_inventory);
        }

        public bool Add(EquipmentInfo equipment)
        {
            if (!_inventory.Add(equipment)) return false;
            return true;
        }

        public bool Remove(EquipmentInfo equipment)
        {
            if (!_inventory.Remove(equipment)) return false;
            return true;
        }

        public bool Add(NftEquipment equipment)
        {
            _inventory.NftEquipments.Add(equipment);
            return true;
        }

        public bool Remove(NftEquipment equipment)
            => _inventory.NftEquipments.Remove(equipment);

        public bool Add(ConsumableInfo consumable)
        {
            if (!_inventory.Add(consumable)) return false;
            return true;
        }

        public bool Remove(ConsumableInfo consumable)
        {
            var result = _inventory.Remove(consumable);
            return result;
        }

        public bool Contains(EquipmentInfo equipment) => _inventory.Equipments.Contains(equipment);

        public bool Contains(NftEquipment equipment) => _inventory.NftEquipments.Contains(equipment);
    }
}