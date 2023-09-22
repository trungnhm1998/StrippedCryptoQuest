using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
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

        public void RegisterCharacterEquipmentsEvents(Equipments charSpecEquipments)
        {
            charSpecEquipments.EquipmentAdded += UpdateEquipmentUI;
            charSpecEquipments.EquipmentRemoved += UpdateEquipmentUI;
        }

        private void UpdateEquipmentUI(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (eTypes.Contains(SlotType))
                Init(equipment);
        }

        public void RemoveCharacterEquipmentsEvents(Equipments charSpecEquipments)
        {
            charSpecEquipments.EquipmentAdded -= UpdateEquipmentUI;
            charSpecEquipments.EquipmentRemoved -= UpdateEquipmentUI;
        }
    }
}