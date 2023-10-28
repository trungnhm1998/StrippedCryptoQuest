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
using IndiGames.Core.SaveSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [Serializable]
    class ConsumableData
    {
        public string Guid;
        public uint Id;
        public int Quantity;
    }

    [Serializable]
    class EquipmentData
    {
        public string DefGuid;
        public string PrefabGuid;
        public uint Id;
        public string DefinitionId;
        public int Level;
    }

    [Serializable]
    class InventoryData
    {
        public List<EquipmentData> Equipments = new();
        public List<ConsumableData> Consumables = new();
    }

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

            _consumables.Add(new ConsumableInfo(item.Data, quantity));

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
                    _consumables.RemoveAt(index);
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

        public string ToJson()
        {
            var inventoryData = new InventoryData();
            foreach (var consumable in Consumables)
            {
                if (consumable != null && consumable.Data != null)
                {
                    var consumableData = new ConsumableData();
                    consumableData.Guid = consumable.Data.Guid;
                    consumableData.Id = consumable.Id;
                    consumableData.Quantity = consumable.Quantity;
                    inventoryData.Consumables.Add(consumableData);
                }
            }
            foreach (var equipment in Equipments)
            {
                if (equipment != null)
                {
                    var equipmentData = new EquipmentData();
                    if (equipment.Def != null)
                    {
                        equipmentData.DefGuid = equipment.Def.Guid;
                    }
                    if (equipment.Prefab != null)
                    {
                        equipmentData.PrefabGuid = equipment.Prefab.Guid;
                    }
                    equipmentData.Id = equipment.Id;
                    equipmentData.DefinitionId = equipment.DefinitionId;
                    equipmentData.Level = equipment.Level;
                    inventoryData.Equipments.Add(equipmentData);
                }
            }
            return JsonUtility.ToJson(inventoryData);
        }

        public IEnumerator CoFromJson(string json)
        {
            Consumables.Clear();
            Equipments.Clear();

            var inventoryData = new InventoryData();
            JsonUtility.FromJsonOverwrite(json, inventoryData);
            foreach (var consumableData in inventoryData.Consumables)
            {
                var dataSoHandle = Addressables.LoadAssetAsync<ConsumableSO>(consumableData.Guid);
                yield return dataSoHandle;
                if (dataSoHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var consumable = new ConsumableInfo(dataSoHandle.Result, consumableData.Quantity)
                    {
                        Id = consumableData.Id
                    };
                    Consumables.Add(consumable);
                }
            }
            foreach (var equipmentData in inventoryData.Equipments)
            {
                var equipment = new EquipmentInfo(equipmentData.DefinitionId, equipmentData.Level)
                {
                    Id = equipmentData.Id
                };
                if (!string.IsNullOrEmpty(equipmentData.DefGuid))
                {
                    var defSoHandle = Addressables.LoadAssetAsync<EquipmentDef>(equipmentData.DefGuid);
                    yield return defSoHandle;
                    if (defSoHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        equipment.Def = defSoHandle.Result;
                    }
                }
                if (!string.IsNullOrEmpty(equipmentData.PrefabGuid))
                {
                    var prefabSoHandle = Addressables.LoadAssetAsync<EquipmentPrefab>(equipmentData.PrefabGuid);
                    yield return prefabSoHandle;
                    if (prefabSoHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        equipment.Prefab = prefabSoHandle.Result;
                    }
                }
                Equipments.Add(equipment);
            }
        }
    }
}