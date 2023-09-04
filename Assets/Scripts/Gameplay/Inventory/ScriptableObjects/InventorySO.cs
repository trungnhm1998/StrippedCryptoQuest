using System.Collections.Generic;
using CryptoQuest.Config;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEditor;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private InventoryConfigSO _inventoryConfig;
        [field: SerializeField] public List<UsableInfo> UsableItems { get; private set; }
        [field: SerializeField] public List<EquipmentInfo> Equipments { get; private set; } = new();

        /// <summary>
        /// This is inventory for equipment
        /// and make management by compartments and for easy-to-work UI
        /// </summary>
        [SerializeField] private List<InventoryContainer> _inventories;

        private Dictionary<EEquipmentCategory, int> _inventoriesCache = new();

        #region Inventory Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateInventory();
        }

        private void ValidateInventory()
        {
            ValidateListLength(ref _inventories, _inventoryConfig.SlotTypeIndex);
            ValidateInventoryContainers();
        }

        private void ValidateInventoryContainers()
        {
            for (int index = 0; index < _inventoryConfig.SlotTypeIndex; index++)
            {
                _inventories[index] ??= new();
                _inventories[index].CurrentItems ??= new();
                _inventories[index].EquipmentCategory = (EEquipmentCategory)index;
            }
        }

        private void ValidateListLength<T>(ref List<T> list, int expectedLength) where T : new()
        {
            list.Capacity = expectedLength;
            while (list.Count < expectedLength)
            {
                list.Add(new T());
            }
        }

        /// <summary>
        /// This method will get the cache of inventory slots to check if the slot is available
        /// in unit test
        /// <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectInventorySlots"/>
        /// </summary>
        /// <returns></returns>
        public int Editor_GetInventorySlotsCacheCount()
        {
            return _inventoriesCache.Count;
        }

        /// <summary>
        /// This method is used for Editor only in unit test
        /// <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectCategoryInSubInventories"/>
        /// </summary>
        /// <returns></returns>
        public List<InventoryContainer> Editor_GetSubInventoryContainers()
        {
            return _inventories;
        }

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
            ValidateInventory();
#endif

            Initialize();
        }

        private void Initialize()
        {
            _inventoriesCache.Clear();

            for (var index = 0; index < _inventories.Count; index++)
            {
                var inventory = _inventories[index];
                _inventoriesCache[inventory.EquipmentCategory] = index;
            }
        }
        

        #region Equipment

        public void Add(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false)
            {
                Debug.LogWarning($"Equipment is null or invalid");
                return;
            }

            Equipments.Add(equipment);
        }

        /// <summary>
        /// How can we know which equipment to remove?
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public void Remove(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false)
            {
                Debug.LogWarning($"Equipment is null or invalid");
                return;
            }

            Equipments.Remove(equipment);
        }

        public bool Add(UsableInfo item, int quantity = 1)
        {
            if (quantity <= 0)
            {
                Debug.LogWarning($"Quantity is less than 0");
                return false;
            }

            if (item == null)
            {
                Debug.LogWarning($"Item is null");
                return false;
            }

            if (item.Data == null)
            {
                Debug.LogWarning($"Item doesn't have item");
                return false;
            }

            item.SetQuantity(item.Quantity + quantity);

            UsableItems.Add(item);

            return true;
        }

        public bool Remove(UsableInfo item, int quantity = 1)
        {
            if (quantity <= 0) return false;

            item.SetQuantity(item.Quantity - quantity);

            if (item.Quantity <= 0)
            {
                UsableItems.Remove(item);
            }

            return true;
        }

        /// <summary>
        /// Get the up to date equipments in inventory using their category <see cref="EEquipmentCategory"/> and <see cref="EquipmentTypeSO"/>
        /// </summary>
        /// <param name="equipmentCategory">type of equipments</param>
        /// <param name="equipments">list of <see cref="EquipmentInfo"/> that will be populated</param>
        /// <returns>list of <see cref="EquipmentInfo"/> in inventory</returns>
        public bool GetEquipmentByType(EEquipmentCategory equipmentCategory, out List<EquipmentInfo> equipments)
        {
            equipments = new();

            if (!_inventoriesCache.TryGetValue(equipmentCategory, out var index)) return false;

            equipments = _inventories[index].CurrentItems;

            return true;
        }


        private bool UpdateInventorySlot(EEquipmentCategory category, EquipmentInfo equipment = null)
        {
            if (!_inventoriesCache.TryGetValue(category, out var cachedIndex))
            {
                Debug.LogWarning($"Inventory doesn't have {category} slot");
                return false;
            }

            var inventory = _inventories[cachedIndex];
            inventory.CurrentItems.Add(equipment);
            return true;
        }

        #endregion
    }
}