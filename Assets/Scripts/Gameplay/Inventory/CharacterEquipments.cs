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
        public event Action<EquipmentInfo, List<ESlotType>> EquipmentAdded;
        public event Action<EquipmentInfo, List<ESlotType>> EquipmentRemoved;

        [field: SerializeField] public List<EquipmentSlot> Slots { get; private set; } = new();

        /// <summary>
        /// Clear when init character to prevent not unsubscribe correctly
        /// </summary>
        public void ClearEventRegistration()
        {
            EquipmentAdded = null;
            EquipmentRemoved = null;
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

            OnEquipmentAdded(equipmentInfo, new List<ESlotType>(requiredSlots));
        }

        /// <summary>
        /// Unequip the equipment in all required slots, and raise an event,
        /// InventoryController, or some manager should listen to this event and add the equipment back to inventory
        /// </summary>
        /// <param name="equipment">This equipment should already in <see cref="Slots"/> or <see cref="_slotsCache"/></param>
        public void Unequip(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;

            var equippedSlots = new List<ESlotType>(); // Support 1 item equip to multiple slots, two handed weapon
            var requiredSlots = equipment.RequiredSlots;
            foreach (var slot in requiredSlots)
            {
                var equipmentInSlot = GetEquipmentInSlot(slot);
                if (equipmentInSlot.IsValid() && equipmentInSlot == equipment)
                {
                    SetEquipmentInSlot(new EquipmentInfo(), slot);
                    equippedSlots.Add(slot);
                }
            }

            OnEquipmentRemoved(equipment, equippedSlots);
        }

        private void OnEquipmentRemoved(EquipmentInfo equipment, List<ESlotType> equippedSlots)
        {
            EquipmentRemoved?.Invoke(equipment, equippedSlots);
        }

        public EquipmentInfo GetEquipmentInSlot(ESlotType slotType)
        {
            // Using dictionary to cache cause me 30min debugging a weird bug so...
            foreach (var equipmentSlot in Slots)
            {
                if (equipmentSlot.Type == slotType)
                    return equipmentSlot.Equipment;
            }

            Debug.Log($"No slot {slotType} found, create new slot");
            var slot = new EquipmentSlot()
            {
                Type = slotType,
                Equipment = new EquipmentInfo()
            };
            Slots.Add(slot);
            return slot.Equipment;
        }

        private void OnEquipmentAdded(EquipmentInfo equipment, List<ESlotType> equippedSlots)
        {
            ESlotType[] requiredSlots = equipment.RequiredSlots;
            foreach (var slot in requiredSlots)
            {
                SetEquipmentInSlot(equipment, slot);
            }

            EquipmentAdded?.Invoke(equipment, equippedSlots);
        }

        private void SetEquipmentInSlot(EquipmentInfo equipment, ESlotType slotType)
        {
            for (var index = 0; index < Slots.Count; index++)
            {
                var slot = Slots[index];
                if (slot.Type == slotType)
                {
                    slot.Equipment = equipment;
                    Slots[index] = slot;
                    return;
                }
            }

            var equipmentSlot = new EquipmentSlot()
            {
                Equipment = equipment,
                Type = slotType
            };
            Slots.Add(equipmentSlot);
        }
    }
}