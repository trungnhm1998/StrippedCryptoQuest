using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        public event Action<EquipmentInfo> Inspecting;
        public event Action<EquipmentInfo> EquipItem;

        [SerializeField] private UIEquipment _equipmentUI;

        [SerializeField] private MultiInputButton _button;
        public EquipmentInfo Equipment => _equipmentUI.Equipment;
        private bool _canClick;

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
            _canClick = true;
            _equipmentUI.Init(equipment);
        }

        public void DeactivateButton()
        {
            _canClick = false;
            _equipmentUI.DisableButton();
        }

        public void OnEquip()
        {
            if (!_canClick) return;
            EquipItem?.Invoke(_equipmentUI.Equipment);
        }
    }
}