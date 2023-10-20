#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using CryptoQuest.Config;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using ESlotType =
    CryptoQuest.Item.Equipment.EquipmentSlot.EType;
using IndiGames.Core.SaveSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : SerializableScriptableObject
    {
        public event Action Loaded;
        [SerializeField] private InventoryConfigSO _inventoryConfig;

        [SerializeField] private List<ConsumableInfo> _consumables = new();
        public List<ConsumableInfo> Consumables => _consumables;

        [SerializeField] private List<EquipmentInfo> _equipments = new();
        public List<EquipmentInfo> Equipments => _equipments;

        public WalletControllerSO WalletController => _walletController;
        [SerializeField] private WalletControllerSO _walletController;

        public void OnLoaded() => Loaded?.Invoke();

        #region Inventory Editor

#if UNITY_EDITOR
        /// <summary>
        /// This function only use for Unit test to get the inventory config
        /// <see cref="InventorySO.OnEnable"/>
        /// </summary>
        private void Editor_ValidateInventoryConfig()
        {
            if (_inventoryConfig != null) return;

            var guids = AssetDatabase.FindAssets("t:InventoryConfigSO");

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var inventoryConfigSO = AssetDatabase.LoadAssetAtPath<InventoryConfigSO>(path);

            _inventoryConfig = inventoryConfigSO;
        }
#endif

        #endregion

        private void OnEnable()
        {
#if UNITY_EDITOR
            Editor_ValidateInventoryConfig();
#endif
        }

        #region Equipment

        public bool Add(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false)
            {
                Debug.LogWarning($"Equipment is null or invalid");
                return false;
            }

            _equipments.Add(equipment);
            return true;
        }

        public bool Remove(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false)
            {
                Debug.LogWarning($"Equipment is null or invalid");
                return false;
            }

            return _equipments.Remove(equipment);
        }

        public bool Add(ConsumableInfo item, int quantity = 1)
        {
            if (item == null || item.IsValid() == false)
            {
                Debug.LogWarning($"Unable to add invalid item");
                return false;
            }

            foreach (var usableItem in Consumables)
            {
                if (usableItem.Data == item.Data)
                {
                    usableItem.SetQuantity(usableItem.Quantity + quantity);
                    return true;
                }
            }

            Consumables.Add(new ConsumableInfo(item.Data, quantity));

            return true;
        }

        public bool Remove(ConsumableInfo item, int quantity = 1)
        {
            if (quantity <= 0) return false;

            if (item == null || !item.IsValid())
            {
                Debug.LogWarning($"Item is null");
                return false;
            }

            for (var index = 0; index < Consumables.Count; index++)
            {
                var consumable = Consumables[index];
                if (consumable.Data != item.Data) continue;
                var consumableQuantity = consumable.Quantity - quantity;
                if (consumableQuantity <= 0)
                {
                    Debug.LogWarning($"Try to remove more {consumable.Data} than you have" +
                                     $"\ncurrent {consumable.Quantity} removing quantity {quantity}");
                    consumable.SetQuantity(0);
                    Consumables.RemoveAt(index);
                }
                else
                {
                    consumable.SetQuantity(consumableQuantity);
                }

                return true;
            }

            Debug.Log($"Try to remove consumable that wasn't found in the {name}");
            return false;
        }

        public void Add(CurrencyInfo currency)
        {
            if (currency == null || !currency.IsValid())
            {
                Debug.LogWarning($"Currency is null or invalid");
                return;
            }

            WalletController.UpdateCurrencyAmount(currency.Data, currency.Amount);
        }

        public void Remove(CurrencyInfo currency)
        {
            if (currency.Amount < 0)
                Add(currency);
        }

        #endregion

#if UNITY_EDITOR
        public void Editor_Add(EquipmentInfo equipment)
        {
            _equipments.Add(equipment);
        }
#endif
    }
}