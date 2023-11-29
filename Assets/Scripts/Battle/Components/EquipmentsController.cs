﻿using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using ESlotType =
    CryptoQuest.Item.Equipment.EquipmentSlot.EType;
using ECategory = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type.EEquipmentCategory;

namespace CryptoQuest.Battle.Components
{
    public interface IEquipmentsProvider
    {
        Equipments GetEquipments();
    }

    public class EquipmentsController : CharacterComponentBase
    {
        public event Action<IEquipment> Equipped;
        public event Action<IEquipment> Removed;

        private Equipments _equipments;
        public Equipments Equipments => _equipments;

        private List<EquipmentSlot> Slots => _equipments.Slots;

        public override void Init()
        {
            var provider = Character.GetComponent<IEquipmentsProvider>();
            _equipments = provider.GetEquipments();
        }

        public IEquipment GetEquipmentInSlot(ESlotType slotType)
        {
            foreach (var equipmentSlot in _equipments.Slots)
            {
                if (equipmentSlot.Type == slotType)
                    return equipmentSlot.Equipment;
            }

            Debug.Log($"No slot {slotType} found, create new slot");
            var slot = new EquipmentSlot()
            {
                Type = slotType,
                Equipment = default
            };
            _equipments.Slots.Add(slot);
            return slot.Equipment;
        }

        /// <summary>
        /// <para>First check if this equipment allow to equip in this slot</para>
        /// <para>Then find all required slots for this equipment
        /// remove all equipment that in the required slots and put in back to inventory
        /// equip the equipment
        /// the required slots will now occupied by the same equipment (check GUID)
        /// apply the effect</para>
        /// </summary>
        public void Equip(IEquipment equipment, ESlotType equippingSlot)
        {
            if (equipment.IsValid() == false) return;

            var allowedSlots = equipment.AllowedSlots;
            if (allowedSlots.Contains(equippingSlot) == false) return;
            var requiredSlots = equipment.RequiredSlots;
            Unequip(GetEquipmentInSlot(equippingSlot));
            OnEquipmentAdded(equipment, equippingSlot, requiredSlots);
        }

        private void OnEquipmentAdded(IEquipment equipment, ESlotType equippingSlot, ESlotType[] requiredSlots)
        {
            SetEquipmentInSlot(equipment, equippingSlot);
            foreach (var slot in requiredSlots)
                if (equippingSlot != slot)
                    SetEquipmentInSlot(equipment, slot);

            Equipped?.Invoke(equipment);
        }

        private void SetEquipmentInSlot(IEquipment equipment, ESlotType slotType)
        {
            for (var index = 0; index < Slots.Count; index++)
            {
                var slot = Slots[index];
                if (slot.Type != slotType) continue;
                slot.Equipment = equipment;
                Slots[index] = slot;
                return;
            }

            var equipmentSlot = new EquipmentSlot()
            {
                Equipment = equipment,
                Type = slotType
            };
            Slots.Add(equipmentSlot);
        }

        public void Unequip(ESlotType slotToUnequip) => Unequip(GetEquipmentInSlot(slotToUnequip));

        /// <summary>
        /// Unequip the equipment in all required slots, and raise an event,
        /// InventoryController, or some manager should listen to this event and add the equipment back to inventory
        /// </summary>
        /// <param name="equipment">This equipment should already in <see cref="Slots"/></param>
        public void Unequip(IEquipment equipment)
        {
            if (equipment == null || equipment.IsValid() == false) return;
            // DO NOT USE FOR EACH due to SetEquipmentInSlot will modify the list cause IEnumerable exception
            for (var index = 0; index < _equipments.Slots.Count; index++)
            {
                var slot = _equipments.Slots[index];
                if (slot.IsValid()
                    // This handle the case when the equipment is in multiple slots
                    && ReferenceEquals(slot.Equipment, equipment))
                {
                    SetEquipmentInSlot(null, slot.Type);
                }
            }

            OnEquipmentRemoved(equipment);
        }

        private void OnEquipmentRemoved(IEquipment equipment) => Removed?.Invoke(equipment);
    }
}