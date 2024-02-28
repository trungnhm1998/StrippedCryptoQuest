﻿using System;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        public event Action<UIEquipmentItem> Inspecting;
        public event Action<UIEquipmentItem> EquipItem;

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
                Inspecting?.Invoke(this);
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
            EquipItem?.Invoke(this);
        }
    }
}