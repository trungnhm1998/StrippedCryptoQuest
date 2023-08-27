using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory
{
    /// <summary>
    /// Using struct here cause the events to be null
    /// </summary>
    [Serializable]
    public class CharacterEquipments
    {
        public event Action<EquipmentInfo> EquipmentAdded;
        public event Action<EquipmentInfo> EquipmentRemoved;

        [field: SerializeField] public List<EquipmentSlot> Slots { get; private set; }
        private Dictionary<ESlotType, int> _slotsCache; // this should only have maximum 8 items

        private Dictionary<ESlotType, int> SlotCache
        {
            get
            {
                if (_slotsCache == null)
                {
                    _slotsCache = new();
                    for (var index = 0; index < Slots.Count; index++)
                    {
                        var slot = Slots[index];
                        _slotsCache.Add(slot.Type, index);
                    }
                }

                return _slotsCache;
            }
        }


        /// <summary>
        /// Find all required slots for this equipment
        /// remove all equipment that in the required slots and put in back to inventory
        /// equip the equipment
        /// the required slots will now occupied by the same equipment (check GUID)
        /// apply the effect
        /// </summary>
        public void Equip(EquipmentInfo equipmentInfo)
        {
            if (equipmentInfo.IsValid() == false) return;

            var requiredSlots = equipmentInfo.RequiredSlots;
            foreach (var slot in requiredSlots)
                Unequip(GetEquipmentInSlot(slot));

            OnEquipmentAdded(equipmentInfo);
        }

        /// <summary>
        /// Unequip the equipment in all required slots, and raise an event,
        /// InventoryController, or some manager should listen to this event and add the equipment back to inventory
        /// </summary>
        /// <param name="equipment">This equipment should already in <see cref="Slots"/> or <see cref="_slotsCache"/></param>
        public void Unequip(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;

            var requiredSlots = equipment.RequiredSlots;
            foreach (var slot in requiredSlots)
            {
                var equipmentInSlot = GetEquipmentInSlot(slot);
                if (equipmentInSlot.IsValid() && equipmentInSlot == equipment)
                    SetEquipmentInSlot(new EquipmentInfo(), slot);
            }

            OnEquipmentRemoved(equipment);
        }

        private void OnEquipmentRemoved(EquipmentInfo equipment)
        {
            EquipmentRemoved?.Invoke(equipment);
        }

        public EquipmentInfo GetEquipmentInSlot(ESlotType slotType)
        {
            if (!SlotCache.TryGetValue(slotType, out var idx))
            {
                var slot = new EquipmentSlot()
                {
                    Type = slotType,
                    Equipment = new EquipmentInfo()
                };
                Slots.Add(slot);
                return slot.Equipment;
            }

            return Slots[idx].Equipment;
        }

        private void OnEquipmentAdded(EquipmentInfo equipment)
        {
            ESlotType[] requiredSlots = equipment.RequiredSlots;
            foreach (var slot in requiredSlots)
            {
                SetEquipmentInSlot(equipment, slot);
            }

            EquipmentAdded?.Invoke(equipment);
        }

        private void SetEquipmentInSlot(EquipmentInfo equipment, ESlotType slotType)
        {
            if (!SlotCache.TryGetValue(slotType, out var idx))
            {
                var equipmentSlot = new EquipmentSlot()
                {
                    Equipment = equipment,
                    Type = slotType
                };
                Slots.Add(equipmentSlot);
                SlotCache.Add(slotType, Slots.Count - 1);
            }
            else
            {
                var slot = Slots[idx];
                slot.Equipment = equipment;
                Slots[idx] = slot;
            }
        }
    }
}