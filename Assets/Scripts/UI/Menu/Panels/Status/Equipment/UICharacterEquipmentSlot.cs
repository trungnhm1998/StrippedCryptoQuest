using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType> ShowEquipmentsInventoryWithType;
        [field: SerializeField] public EquipmentSlot.EType SlotType { get; private set; }

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
            ShowEquipmentsInventoryWithType?.Invoke(SlotType);
        }

        public void RegisterCharacterEquipmentsEvents(CharacterEquipments charSpecEquipments)
        {
            charSpecEquipments.EquipmentAdded += UpdateEquipmentUI;
            charSpecEquipments.EquipmentRemoved += UpdateEquipmentUI;
        }

        private void UpdateEquipmentUI(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (eTypes.Contains(SlotType))
                Init(equipment);
        }

        public void RemoveCharacterEquipmentsEvents(CharacterEquipments charSpecEquipments)
        {
            charSpecEquipments.EquipmentAdded -= UpdateEquipmentUI;
            charSpecEquipments.EquipmentRemoved -= UpdateEquipmentUI;
        }
    }
}