using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentOverview : MonoBehaviour
    {
        public event Action<EquipmentFilters> EquipmentSlotSelected;

        [Header("Game Components")]
        [SerializeField] private List<UIEquipmentSlotButton> _equipmentSlots;
        [SerializeField] private List<GameObject> _itemContainers;
        [SerializeField] private GameObject _item;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;

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

        private void EquipmentSlotPressed(EquipmentFilters filters)
        {
            EquipmentSlotSelected?.Invoke(filters);
            DisableAllButtons();
        }

        public void SetEquipment(CharacterEquipments currentEquipping)
        {
            RefreshUI();

            var equipment = currentEquipping.GetEquippingSlots();
            var i = 0;

            foreach (var slot in equipment)
            {
                if (slot.Equipment.Item != null)
                {
                    Debug.Log($"@@@@@@@@@@@@@@@@@@@@@");
                    var item = Instantiate(_item, _itemContainers[i].transform);
                    item.GetComponent<UIEquipmentItem>().Init(slot.Equipment);
                }
                i++;
            }
        }

        private void RefreshUI()
        {
            foreach (var row in _itemContainers)
            {
                foreach (Transform child in row.transform) {
                    Destroy(child.gameObject);
                }
            }
        }

        #region State context
        public void Init()
        {
            EnableAllButtons();
            _defaultSelection.Select();
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
        }
        #endregion
    }
}