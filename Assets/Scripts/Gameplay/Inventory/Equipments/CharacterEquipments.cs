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
        [NonReorderable, SerializeField] private List<EquippingSlotContainer> _equippingSlots = new();
        private Dictionary<ESlotType, EquippingSlotContainer> _equippingSlotsCache = new();

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

        public EquippingSlotContainer GetSlot(ESlotType slotType)
        {
            return _equippingSlotsCache[slotType];
        }

        public bool UpdateEquippingSlot(ESlotType slotType, EquipmentInfo equipmentInfo = null)
        {
            if (!_equippingSlotsCache.TryGetValue(slotType, out var slot))
            {
                Debug.LogWarning($"Slot {slotType} is not available");
                return false;
            }

            slot.UpdateEquipment(equipmentInfo);
            return true;
        }

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
    }
}