using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class InventoryControllerSO : ScriptableObject, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private WalletSO _wallet;
        public InventorySO Inventory => _inventory;

        private void OnEnable()
        {
            ServiceProvider.Provide<IInventoryController>(this);
        }

        public bool Add(Equipment equipment)
        {
            if (equipment == null || equipment.IsValid() == false)
            {
                Debug.LogWarning($"Equipment is null or invalid");
                return false;
            }

            _inventory.Equipments.Add(equipment);
            return true;
        }

        public bool Remove(Equipment equipment)
        {
            if (equipment != null && equipment.IsValid()) return _inventory.Equipments.Remove(equipment);
            Debug.LogWarning($"Equipment is null or invalid");
            return false;
        }

        public bool Add(NftEquipment equipment)
        {
            _inventory.NftEquipments.Add(equipment);
            return true;
        }

        public bool Remove(NftEquipment equipment)
            => _inventory.NftEquipments.Remove(equipment);

        public bool Add(ConsumableInfo item, int quantity = 1)
        {
            if (item == null || item.IsValid() == false)
            {
                Debug.LogWarning($"Unable to add invalid item");
                return false;
            }

            foreach (var usableItem in _inventory.Consumables)
            {
                if (usableItem.Data == item.Data)
                {
                    usableItem.SetQuantity(usableItem.Quantity + quantity);
                    return true;
                }
            }

            _inventory.Consumables.Add(new ConsumableInfo(item.Data, quantity));

            return true;
        }

        public bool Remove(ConsumableInfo item, int quantity = 1)
        {
            if (quantity <= 0 || item == null || !item.IsValid())
            {
                Debug.LogWarning("InventoryControllerSO::RemoveConsumable invalid arguments");
                return false;
            }

            for (var index = 0; index < _inventory.Consumables.Count; index++)
            {
                var consumable = _inventory.Consumables[index];
                if (consumable.Data != item.Data) continue;
                var newQuantity = consumable.Quantity - quantity;
                if (newQuantity <= 0)
                {
                    Debug.LogWarning($"Try to remove more {consumable.Data} than you have" +
                                     $"\ncurrent {consumable.Quantity} removing quantity {quantity}");
                    consumable.SetQuantity(0);
                    _inventory.Consumables.RemoveAt(index);
                }
                else
                {
                    consumable.SetQuantity(newQuantity);
                }

                return true;
            }

            Debug.Log($"Try to remove consumable {item.Data.name} that wasn't found in the {name}");
            return false;
        }

        public bool Contains(Equipment equipment) => _inventory.Equipments.Contains(equipment);

        public bool Contains(NftEquipment equipment) => _inventory.NftEquipments.Contains(equipment);

        public bool Add(CurrencyInfo currency)
        {
            if (currency == null || !currency.IsValid())
            {
                Debug.LogWarning($"Currency is null or invalid");
                return false;
            }

            var newAmount = _wallet[currency.Data].Amount + currency.Amount;
            _wallet[currency.Data].SetAmount(newAmount);
            return true;
        }

        public bool Remove(CurrencyInfo currency)
        {
            if (currency == null || !currency.IsValid())
            {
                Debug.LogWarning($"Currency is null or invalid");
                return false;
            }

            var newAmount = _wallet[currency.Data].Amount - currency.Amount;
            if (newAmount < 0)
            {
                Debug.LogWarning($"Insufficient funds!");
                return false;
            }

            _wallet[currency.Data].SetAmount(newAmount);
            return true;
        }
    }
}