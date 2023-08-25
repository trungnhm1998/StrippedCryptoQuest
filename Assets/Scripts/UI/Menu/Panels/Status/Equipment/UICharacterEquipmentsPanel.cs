using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        public event Action<EEquipmentCategory> EquipmentSlotSelected;

        [Header("Game Components")]
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> _equipmentSlotsCache = null;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> EquipmentSlots
        {
            get
            {
                if (_equipmentSlotsCache == null)
                {
                    _equipmentSlotsCache = new Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot>();
                    foreach (var equipmentSlot in _equipmentSlots)
                    {
                        _equipmentSlotsCache.Add(equipmentSlot.SlotType, equipmentSlot);
                    }
                }

                return _equipmentSlotsCache;
            }
        }

        private void OnValidate()
        {
            _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
        }

        private void OnEnable() { }

        private void OnDisable() { }

        private void EquipmentSlotPressed(EEquipmentCategory category)
        {
            EquipmentSlotSelected?.Invoke(category);
            DisableAllButtons();
        }

        public void SetEquipment(CharacterEquipments characterEquipments)
        {
            ResetEquipments();
            for (int i = 0; i < characterEquipments.Slots.Count; i++)
            {
                var equipmentSlot = characterEquipments.Slots[i];
                if (equipmentSlot.IsValid() == false) continue;
                var uiEquipmentSlot = EquipmentSlots[equipmentSlot.Type];
                uiEquipmentSlot.Init(equipmentSlot.Equipment);
            }
        }
        
        private void ResetEquipments()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Reset();
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