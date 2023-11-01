using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType, EEquipmentCategory> ShowEquipmentsInventoryWithType;
        [field: SerializeField] public EquipmentSlot.EType SlotType { get; private set; }
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; private set; }

        [SerializeField] private UIEquipment _equipment;

        public void Init(EquipmentInfo equipmentSlotEquipment)
        {
            if (!equipmentSlotEquipment.IsValid()) return;
            _equipment.Init(equipmentSlotEquipment);
            _equipment.gameObject.SetActive(true);
        }

        public void Reset()
        {
            _equipment.Reset();
            _equipment.gameObject.SetActive(false);
        }

        public void OnChangingEquipment()
        {
            ShowEquipmentsInventoryWithType?.Invoke(SlotType, EquipmentCategory);
        }

        private void UpdateEquipmentUI(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (eTypes.Contains(SlotType))
                Init(equipment);
        }
    }
}