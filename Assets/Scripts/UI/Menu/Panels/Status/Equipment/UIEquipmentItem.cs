using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        public event Action<EquipmentInfo> Inspecting;
        public event Action<EquipmentInfo> EquipItem;
        [FormerlySerializedAs("_equipment")]
        [SerializeField] private UIEquipment _equipmentUI;
        [SerializeField] private MultiInputButton _button;
        public EquipmentInfo Equipment => _equipmentUI.Equipment;

        private void OnEnable()
        {
            _button.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
        }

        private void OnSelected()
        {
            if (_equipmentUI.Equipment.IsValid())
                Inspecting?.Invoke(_equipmentUI.Equipment);
        }

        public void Init(EquipmentInfo equipment)
        {
            _equipmentUI.Init(equipment);
        }

        public void OnEquip()
        {
            EquipItem?.Invoke(_equipmentUI.Equipment);
        }
    }
}