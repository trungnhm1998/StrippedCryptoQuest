using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public struct CharacterEquipments
    {
        public event Action<ESlotType, EquipmentSlot> EquipmentAdded;
        public event Action<ESlotType, EquipmentSlot> EquipmentRemoved;

        [field: SerializeField] public List<EquipmentSlot> Slots { get; private set; }
        private Dictionary<ESlotType, EquipmentSlot>
            _equippingSlotsCache; // even though there should only 8 Slots I want faster lookup

        public List<EquipmentSlot> GetEquippingSlots()
        {
            return Slots;
        }

        public bool Equip(ESlotType slot, EquipmentInfo equipmentInfo)
        {
            if (!IsSlotValid(slot, equipmentInfo))
            {
                return false;
            }

            OnEquipmentAdd(slot, equipmentInfo);
            return true;
        }

        private void OnEquipmentAdd(ESlotType slot, EquipmentInfo equipmentInfo)
        {
            var equipmentSlot = new EquipmentSlot()
            {
                Equipment = equipmentInfo,
                Type = slot
            };
            Slots.Add(equipmentSlot);
            _equippingSlotsCache.Add(slot, equipmentSlot);
            EquipmentAdded?.Invoke(slot, equipmentSlot);
        }

        public bool UnEquip(ESlotType slotType, EquipmentInfo equipmentInfo)
        {
            return true;
        }

        /// <summary>
        /// Is this slot available for equipping?
        /// </summary>
        /// <param name="slotType">what slot</param>
        /// <param name="equipmentInfo">what equipment</param>
        /// <returns>return false if already has equipment in slot should <see cref="UnEquip"/> first</returns>
        public bool IsSlotValid(ESlotType slotType, EquipmentInfo equipmentInfo)
        {
            if (equipmentInfo == null || equipmentInfo.IsValid() == false)
            {
                Debug.LogWarning($"CharacterEquipments::IsSlotValid::equipmentInfo is null or invalid");
                return false;
            }

            LazyInitCache();

            if (_equippingSlotsCache.ContainsKey(slotType))
            {
                Debug.LogWarning($"CharacterEquipments::Slot {slotType} is not available");
                return false;
            }

            return true;
        }

        private void LazyInitCache()
        {
            if (_equippingSlotsCache != null) return;

            _equippingSlotsCache = new Dictionary<ESlotType, EquipmentSlot>();
            foreach (var slot in Slots)
            {
                _equippingSlotsCache.Add(slot.Type, slot);
            }
        }
    }
}