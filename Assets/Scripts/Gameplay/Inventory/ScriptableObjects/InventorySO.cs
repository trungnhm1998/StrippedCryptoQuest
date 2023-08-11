using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using ESlotEquipmentContainer =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        // TODO: Make this constant to be able to change in ConfigSO
        public const int EQUIPMENT_SLOTS_COUNT = (int)ESlotEquipmentContainer.Count;
        public const int INVENTORY_SLOTS_COUNT = (int)EEquipmentCategory.Count;

        [NonReorderable, SerializeField] private List<EquippingSlotContainer> _equippingSlots =
            new(EQUIPMENT_SLOTS_COUNT);

        private Dictionary<ESlotEquipmentContainer, EquippingSlotContainer>
            _equippingSlotsCache = new();

        [field: Header("Inventory")]
        [field: SerializeField] public List<UsableInfo> UsableItems { get; private set; }

        /// <summary>
        /// This is sub inventory for equipment
        /// and make management by compartments and for easy-to-work UI
        /// </summary>
        [SerializeField] private List<InventoryContainer> _subInventories =
            new(INVENTORY_SLOTS_COUNT);

        private Dictionary<EEquipmentCategory, int> _subInventoriesCache = new();

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
                _equippingSlots[index].Type = (ESlotEquipmentContainer)index;
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
        public Dictionary<ESlotEquipmentContainer, EquippingSlotContainer> Editor_GetEquippingCache()
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

            for (var index = 0; index < _equippingSlots.Count; index++)
            {
                var slot = _equippingSlots[index];
                if (!_equippingSlotsCache.TryAdd(slot.Type, slot))
                {
                    Debug.LogError($"Equipping slot {slot.Type} is duplicated");
                }
            }

            for (var index = 0; index < _subInventories.Count; index++)
            {
                var inventory = _subInventories[index];
                if (!_subInventoriesCache.TryAdd(inventory.EquipmentCategory, index))
                {
                    Debug.LogError($"Inventory slot {inventory.EquipmentCategory} is duplicated");
                }
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

            if (!_subInventoriesCache.TryGetValue(equipment.Item.EquipmentType.EquipmentCategory, out var cachedIndex))
            {
                Debug.LogWarning($"Inventory doesn't have {equipment.Item.EquipmentType.EquipmentCategory} slot");
                return false;
            }

            var inventory = _subInventories[cachedIndex];
            inventory.CurrentItems.Add(equipment);
            return true;
        }

        public bool Add(EEquipmentCategory equipmentCategory, EquipmentInfo equipment)
        {
            return true;
        }

        // TODO: This function must be private 
        public bool Equip(ESlotEquipmentContainer allowedSlot, EquipmentInfo equipmentInfo)
        {
            if (!Unequip(allowedSlot))
            {
                Debug.LogWarning($"Cannot unequip {allowedSlot}");
                return false;
            }

            if (!UpdateEquippingSlot(allowedSlot, equipmentInfo))
            {
                Debug.LogWarning($"Cannot update inventory {allowedSlot}");
                return false;
            }

            return true;
        }


        // TODO: This function must be private 
        public bool Unequip(ESlotEquipmentContainer slotType)
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

        public bool GetEquipmentByType(EEquipmentCategory equipmentCategory, out List<EquipmentInfo> equipments)
        {
            equipments = new();

            if (!_subInventoriesCache.TryGetValue(equipmentCategory, out var index)) return false;

            equipments = _subInventories[index].CurrentItems;

            return true;
        }

        public EquippingSlotContainer GetInventorySlot(ESlotEquipmentContainer slotType)
        {
            return _equippingSlotsCache[slotType];
        }

        private bool UpdateEquippingSlot(ESlotEquipmentContainer slotType, EquipmentInfo equipmentInfo = null)
        {
            if (!_equippingSlotsCache.TryGetValue(slotType, out var slot))
            {
                Debug.LogError($"Slot {slotType} is not available");
                return false;
            }

            slot.UpdateEquipment(equipmentInfo);
            return true;
        }

        #endregion
    }
}