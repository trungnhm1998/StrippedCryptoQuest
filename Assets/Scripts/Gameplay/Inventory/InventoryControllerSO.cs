using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class InventoryControllerSO : ScriptableObject, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        private void OnEnable()
        {
            ServiceProvider.Provide<IInventoryController>(this);
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