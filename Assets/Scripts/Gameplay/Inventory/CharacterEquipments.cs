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
        [field: SerializeField] public List<EquipmentSlot> Slots { get; set; }
        private Dictionary<ESlotType, EquipmentSlot> _equippingSlotsCache;

        public List<EquipmentSlot> GetEquippingSlots()
        {
            return Slots;
        }

        public bool Equip(ESlotType allowedSlot, EquipmentInfo equipmentInfo)
        {
            if (equipmentInfo == null)
            {
                Debug.LogWarning($"CharacterEquipments::Equipment is null");
                return false;
            }

            /*
            if (!Inventory.Remove(equipmentInfo))
            {
                Debug.LogWarning($"CharacterEquipments::Don't have {equipmentInfo} in inventory");
                return false;
            }
            */

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

            /*
            if (!Inventory.Add(equipmentInfo))
            {
                Debug.LogWarning($"CharacterEquipments::Cannot add {equipmentInfo} to inventory");
                return false;
            }
            */

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