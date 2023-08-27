using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType> UnequipEquipmentAtSlot;
        // refs
        [SerializeField] private UIStatusMenu _mainPanel;
        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;

        // config
        [SerializeField] private GameObject _equipmentSlotParent;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;

        // Tooltips
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> _equipmentSlotsCache = new();

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> EquipmentSlots
        {
            get
            {
                if (_equipmentSlotsCache == null || _equipmentSlotsCache.Count == 0)
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
        }
#endif

        private void OnEnable()
        {
            Show();
            _equipmentsInventoryPanel.UnequipPressed += UnequipCurrentSlot;
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ShowEquipmentsInventoryWithType += ShowEquipmentsInventoryWithType;
            }
        }

        private void OnDisable()
        {
            _equipmentsInventoryPanel.UnequipPressed -= UnequipCurrentSlot;
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ShowEquipmentsInventoryWithType -= ShowEquipmentsInventoryWithType;
            }
        }

        private EquipmentSlot.EType _modifyingSlotType;
        public EquipmentSlot.EType ModifyingSlotType => _modifyingSlotType;

        private void UnequipCurrentSlot()
        {
            Debug.Log($"Unequip equipment at slot {_modifyingSlotType}");
            UnequipEquipmentAtSlot?.Invoke(_modifyingSlotType);
        }

        private void ShowEquipmentsInventoryWithType(EquipmentSlot.EType slotType)
        {
            HideToolTipAndDeselectCurrentSelectedButton();
            _modifyingSlotType = slotType;
            _mainPanel.State.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }

        public void SetEquipmentsUI(CharacterEquipments characterEquipments)
        {
            ResetEquipmentsUI();
            for (int i = 0; i < characterEquipments.Slots.Count; i++)
            {
                var equipmentSlot = characterEquipments.Slots[i];
                if (equipmentSlot.IsValid() == false) continue;
                var uiEquipmentSlot = EquipmentSlots[equipmentSlot.Type];
                uiEquipmentSlot.Init(equipmentSlot.Equipment);
            }

            ReselectCurrentButton();
        }

        /// <summary>
        /// Hide the tooltips
        /// Tooltip will be shown when the button is selected, but while changing tab the button already selected and
        /// show the previous data, so we need to reselect the button to show the correct tooltip
        /// </summary>
        private void ReselectCurrentButton()
        {
            var button = HideToolTipAndDeselectCurrentSelectedButton();
            button.Select();
        }

        private MultiInputButton HideToolTipAndDeselectCurrentSelectedButton()
        {
            _tooltipProvider.Tooltip.Hide();
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            if (currentSelected == null) return null;
            var button = currentSelected.GetComponent<MultiInputButton>();
            if (button == null) return button;
            EventSystem.current.SetSelectedGameObject(null);
            return button;
        }

        private void ResetEquipmentsUI()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Reset();
            }
        }

        #region State context

        public void Show()
        {
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentSlotParent.SetActive(true);
            _defaultSelection.Select();
        }

        public void Hide()
        {
            _tooltipProvider.Tooltip.Hide();
            _equipmentSlotParent.SetActive(false);
        }

        #endregion
    }
}