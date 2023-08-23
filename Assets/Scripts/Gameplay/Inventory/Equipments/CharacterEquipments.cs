using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [Serializable]
    public class CharacterEquipments
    {
        [NonReorderable, SerializeField] private List<EquippingSlotContainer> _equippingSlots;
        private Dictionary<ESlotType, EquippingSlotContainer> _equippingSlotsCache = new();
        public InventorySO Inventory { get; set; }

        public CharacterEquipments() { }

        public void Initialize(int categoryIndex, int slotTypeIndex)
        {
#if UNITY_EDITOR
            ValidateListLength(ref _equippingSlots, categoryIndex);
            ValidateEquippingSlots(categoryIndex, slotTypeIndex);
#endif

            foreach (var slot in _equippingSlots)
            {
                _equippingSlotsCache[slot.Type] = slot;
            }
        }

        #region Validate

#if UNITY_EDITOR

        private void ValidateEquippingSlots(int equipmentCategory, int slotType)
        {
            for (int index = 0; index < equipmentCategory; index++)
            {
                _equippingSlots[index] ??= new EquippingSlotContainer();
                _equippingSlots[index].Equipment ??= new EquipmentInfo();
                _equippingSlots[index].Type = (ESlotType)index;
                _equippingSlots[index].EquipmentCategory =
                    (EEquipmentCategory)Mathf.Clamp(index, 0, slotType - 1);
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
#endif

        #endregion

        #region Editor

#if UNITY_EDITOR

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
        /// This method is used for Editor only in unit test
        /// <see cref="InventorySOTest.Setup"/>
        /// </summary>
        /// <returns></returns>
        public Dictionary<ESlotType, EquippingSlotContainer> Editor_GetEquippingCache()
        {
            return _equippingSlotsCache;
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

        public List<EquippingSlotContainer> GetEquippingSlots()
        {
            return _equippingSlots;
        }

        public bool Equip(ESlotType allowedSlot, EquipmentInfo equipmentInfo)
        {
            if (equipmentInfo == null)
            {
                Debug.LogWarning($"CharacterEquipments::Equipment is null");
                return false;
            }

            if (!Inventory.Remove(equipmentInfo))
            {
                Debug.LogWarning($"CharacterEquipments::Don't have {equipmentInfo} in inventory");
                return false;
            }

            if (!UpdateEquippingSlot(allowedSlot, equipmentInfo))
            {
                Debug.LogWarning($"CharacterEquipments::Cannot update inventory {allowedSlot}");
                return false;
            }

            return true;
        }

        public bool Unequip(ESlotType slotType, EquipmentInfo equipmentInfo)
        {
            if (equipmentInfo == null)
            {
                Debug.LogWarning($"CharacterEquipments::Equipment is null");
                return false;
            }

            if (!Inventory.Add(equipmentInfo))
            {
                Debug.LogWarning($"CharacterEquipments::Cannot add {equipmentInfo} to inventory");
                return false;
            }

            if (!UpdateEquippingSlot(slotType))
            {
                Debug.LogWarning($"CharacterEquipments::Cannot update inventory {slotType}");
                return false;
            }

            return true;
        }

        public bool UpdateEquippingSlot(ESlotType slotType, EquipmentInfo equipmentInfo = null)
        {
            if (!_equippingSlotsCache.TryGetValue(slotType, out var slot))
            {
                Debug.LogWarning($"CharacterEquipments::Slot {slotType} is not available");
                return false;
            }

            slot.UpdateEquipment(equipmentInfo);
            return true;
        }
    }
}