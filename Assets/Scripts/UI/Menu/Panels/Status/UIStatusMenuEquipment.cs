using System;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusMenuEquipment : MonoBehaviour
    {
        public event Action<UIEquipmentSlotButton.EEquipmentType> EquipmentSlotSelected;

        [Header("Game Components")]
        [SerializeField] private List<UIEquipmentSlotButton> _equipmentSlots;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;
        [SerializeField] private GameObject _navigations;

        private void OnEnable()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Pressed += EquipmentSlotPressed;
            }
        }

        private void OnDisable()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Pressed -= EquipmentSlotPressed;
            }
        }

        private void EquipmentSlotPressed(UIEquipmentSlotButton.EEquipmentType equipmentType)
        {
            EquipmentSlotSelected?.Invoke(equipmentType);
            DisableAllButtons();
        }

        public void Init()
        {
            EnableAllButtons();
            _defaultSelection.Select();
            _navigations.SetActive(true);
        }

        /// <summary>
        /// Remove buttons from unity event system
        /// </summary>
        private void DisableAllButtons()
        {
            foreach (var slotButton in _equipmentSlots)
            {
                slotButton.enabled = false;
            }
        }

        /// <summary>
        /// Add buttons to unity event system
        /// </summary>
        private void EnableAllButtons()
        {
            foreach (var slotButton in _equipmentSlots)
            {
                slotButton.enabled = true;
            }
        }

        public void DeInit()
        {
            DisableAllButtons();
            _navigations.SetActive(false);
        }
    }
}