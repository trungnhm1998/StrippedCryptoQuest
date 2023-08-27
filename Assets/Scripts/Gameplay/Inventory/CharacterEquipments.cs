using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public struct CharacterEquipments
    {
        public event Action<EquipmentInfo> EquipmentAdded;
        public event Action<EquipmentInfo> EquipmentRemoved;

        [field: SerializeField] public List<EquipmentSlot> Slots { get; private set; }
        private Dictionary<ESlotType, EquipmentSlot> _slotsCache; // this should only have maximum 8 items

        private Dictionary<ESlotType, EquipmentSlot> SlotCache
        {
            get
            {
                if (_slotsCache == null)
                {
                    _slotsCache = new();
                    foreach (var slot in Slots)
                    {
                        _slotsCache.Add(slot.Type, slot);
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
            if (!SlotCache.TryGetValue(slotType, out var slot))
            {
                slot = new EquipmentSlot()
                {
                    Type = slotType,
                    Equipment = new EquipmentInfo()
                };
            }

            return slot.Equipment;
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

        private void SetEquipmentInSlot(EquipmentInfo equipment, ESlotType slot)
        {
            if (!SlotCache.TryGetValue(slot, out var equipmentSlot))
            {
                equipmentSlot = new EquipmentSlot()
                {
                    Equipment = equipment,
                    Type = slot
                };
                Slots.Add(equipmentSlot);
                SlotCache.Add(slot, equipmentSlot);
            }
            else
            {
                equipmentSlot.Equipment = equipment;
            }
        }
    }
}