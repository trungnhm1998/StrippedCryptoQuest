using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Events;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        // TODO: Make this constant to be able to change in ConfigSO
        public const int EQUIPMENT_SLOTS_COUNT = (int)ESlotType.Count;
        public const int INVENTORY_SLOTS_COUNT = (int)EEquipmentCategory.Count;

        [NonReorderable, SerializeField] private List<EquippingSlotContainer> _equippingSlots =
            new(EQUIPMENT_SLOTS_COUNT);

        private Dictionary<ESlotType, EquippingSlotContainer>
            _equippingSlotsCache = new();

        [field: Header("Inventory")]
        [field: SerializeField] public List<UsableInfo> UsableItems { get; private set; } = new();

        /// <summary>
        /// This is sub inventory for equipment
        /// and make management by compartments and for easy-to-work UI
        /// </summary>
        [SerializeField] private List<InventoryContainer> _subInventories =
            new(INVENTORY_SLOTS_COUNT);

        private Dictionary<EEquipmentCategory, int> _subInventoriesCache = new();

        #region Inventory Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateEquipping();
            ValidateInventory();
        }

        private void ValidateEquipping()
        {
            ValidateListLength(ref _equippingSlots, EQUIPMENT_SLOTS_COUNT);
            ValidateEquippingSlots();
        }

        private void ValidateEquippingSlots()
        {
            for (int index = 0; index < EQUIPMENT_SLOTS_COUNT; index++)
            {
                _equippingSlots[index] ??= new EquippingSlotContainer();
                _equippingSlots[index].Equipment ??= new EquipmentInfo();
                _equippingSlots[index].Type = (ESlotType)index;
                _equippingSlots[index].EquipmentCategory =
                    (EEquipmentCategory)Mathf.Clamp(index, 0, INVENTORY_SLOTS_COUNT - 1);
            }
        }

        private void ValidateInventory()
        {
            ValidateListLength(ref _subInventories, INVENTORY_SLOTS_COUNT);
            ValidateInventoryContainers();
        }

        private void ValidateInventoryContainers()
        {
            for (int index = 0; index < INVENTORY_SLOTS_COUNT; index++)
            {
                _subInventories[index] ??= new InventoryContainer();
                _subInventories[index].CurrentItems ??= new List<EquipmentInfo>();
                _subInventories[index].EquipmentCategory = (EEquipmentCategory)index;
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
        /// This method will get the cache of equipping slots to check if the slot is available
        /// in unit test
        /// <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectSlots"/>
        /// </summary>
        /// <returns></returns>
        public int Editor_GetEquipmentSlotsCount()
        {
            return _equippingSlotsCache.Count;
        }

        /// <summary>
        /// This method will get the cache of inventory slots to check if the slot is available
        /// in unit test
        /// <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectInventorySlots"/>
        /// </summary>
        /// <returns></returns>
        public int Editor_GetInventorySlotsCacheCount()
        {
            return _subInventoriesCache.Count;
        }

        /// <summary>
        /// This method is used for Editor only in unit test
        /// <see cref="InventorySOTest.Setup"/>
        /// </summary>
        /// <returns></returns>
        public Dictionary<ESlotType, EquippingSlotContainer> Editor_GetEquippingCache()
        {
            return _equippingSlotsCache;
        }

        /// <summary>
        /// This method is used for Editor only in unit test
        /// <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectCategoryInSubInventories"/>
        /// </summary>
        /// <returns></returns>
        public List<InventoryContainer> Editor_GetSubInventoryContainers()
        {
            return _subInventories;
        }

        /// <summary>
        ///  This method is used for Editor only in unit test
        ///  <see cref="InventorySOTest.InventorySO_ShouldHaveCorrectSlots"/>
        /// </summary>
        /// <returns></returns>
        public List<EquippingSlotContainer> Editor_GetEquippingSlotContainers()
        {
            return _equippingSlots;
        }
#endif

        #endregion

        private void OnEnable()
        {
#if UNITY_EDITOR
            ValidateEquipping();
            ValidateInventory();
#endif
            Initialize();
        }

        private void Initialize()
        {
            _equippingSlotsCache.Clear();
            _subInventoriesCache.Clear();

            foreach (var slot in _equippingSlots)
            {
                _equippingSlotsCache[slot.Type] = slot;
            }

            for (var index = 0; index < _subInventories.Count; index++)
            {
                var inventory = _subInventories[index];
                _subInventoriesCache[inventory.EquipmentCategory] = index;
            }
        }

        #region Equipment

        public bool Add(EquipmentInfo equipment)
        {
            if (equipment == null)
            {
                Debug.LogWarning($"Equipment is null");
                return false;
            }

            if (equipment.Item == null)
            {
                Debug.LogWarning($"Equipment doesn't have item");
                return false;
            }

            var equipmentCategory = equipment.Item.EquipmentType.EquipmentCategory;

            UpdateInventorySlot(equipmentCategory, equipment);

            return true;
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

            if (item.Item == null)
            {
                Debug.LogWarning($"Item doesn't have item");
                return false;
            }

            item.SetQuantity(item.Quantity + quantity);

            UsableItems.Add(item);

            return true;
        }


        public bool Add(EEquipmentCategory equipmentCategory, EquipmentInfo equipment)
        {
            if (equipment == null)
            {
                Debug.LogWarning($"Equipment is null");
                return false;
            }

            if (equipment.Item == null)
            {
                Debug.LogWarning($"Equipment doesn't have item");
                return false;
            }

            UpdateInventorySlot(equipmentCategory, equipment);

            return true;
        }

        public bool Remove(EquipmentInfo equipment)
        {
            int index = GetInventoryIndex(equipment);
            if (index < 0) return false;

            _subInventories[index].CurrentItems.Remove(equipment);
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

        public bool Equip(ESlotType allowedSlot, EquipmentInfo equipmentInfo)
        {
            if (!Unequip(allowedSlot))
            {
                Debug.LogWarning($"Cannot unequip {allowedSlot}");
                return false;
            }

            if (!Remove(equipmentInfo))
            {
                Debug.LogWarning($"Inventory doesn't have {equipmentInfo.Item.name}");
                return false;
            }

            if (!UpdateEquippingSlot(allowedSlot, equipmentInfo))
            {
                Debug.LogWarning($"Cannot update inventory {allowedSlot}");
                return false;
            }

            return true;
        }

        public bool Unequip(ESlotType slotType)
        {
            if (!UpdateEquippingSlot(slotType))
            {
                Debug.LogWarning($"Cannot update inventory {slotType}");
                return false;
            }

            return true;
        }

        public int CountEquipmentInSlot(EEquipmentCategory slotEquipmentCategory = EEquipmentCategory.Weapon)
        {
            var currentItemsInSlot = _subInventories[(int)slotEquipmentCategory].CurrentItems;

            return currentItemsInSlot.Count;
        }

        private int GetInventoryIndex(EquipmentInfo equipment)
        {
            if (equipment == null)
            {
                Debug.LogWarning($"Equipment is null");
                return -1;
            }

            if (equipment.Item == null)
            {
                Debug.LogWarning($"Equipment doesn't have item");
                return -1;
            }

            var currentCategory = equipment.Item.EquipmentType.EquipmentCategory;
            if (!_subInventoriesCache.TryGetValue(currentCategory, out var index))
            {
                Debug.LogWarning($"Inventory doesn't have {currentCategory}");
                return -1;
            }

            return index;
        }

        public bool GetEquipmentByType(EEquipmentCategory equipmentCategory, out List<EquipmentInfo> equipments)
        {
            equipments = new();

            if (!_subInventoriesCache.TryGetValue(equipmentCategory, out var index)) return false;

            equipments = _subInventories[index].CurrentItems;

            return true;
        }

        public EquippingSlotContainer GetInventorySlot(ESlotType slotType)
        {
            return _equippingSlotsCache[slotType];
        }

        private bool UpdateEquippingSlot(ESlotType slotType, EquipmentInfo equipmentInfo = null)
        {
            if (!_equippingSlotsCache.TryGetValue(slotType, out var slot))
            {
                Debug.LogError($"Slot {slotType} is not available");
                return false;
            }

            slot.UpdateEquipment(equipmentInfo);
            return true;
        }

        private bool UpdateInventorySlot(EEquipmentCategory category, EquipmentInfo equipment = null)
        {
            if (!_subInventoriesCache.TryGetValue(category, out var cachedIndex))
            {
                Debug.LogWarning($"Inventory doesn't have {category} slot");
                return false;
            }

            var inventory = _subInventories[cachedIndex];
            inventory.CurrentItems.Add(equipment);
            return true;
        }

        #endregion
    }
}