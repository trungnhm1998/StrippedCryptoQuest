using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menu;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType> UnequipEquipmentAtSlot;

        // refs
        [SerializeField] private UIStatusMenu _mainPanel;
        [SerializeField] private UIStatusCharacter _characterPanel;
        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;

        // config
        [SerializeField] private GameObject _equipmentSlotParent;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;

        // Tooltips
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private CharacterSpec _currentInspectingCharacter;
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
            _characterPanel.InspectingCharacter += UpdateCharacterEquipments;
            _equipmentsInventoryPanel.UnequipPressed += UnequipCurrentSlot;
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ShowEquipmentsInventoryWithType += ShowEquipmentsInventoryWithType;
            }
        }

        private void OnDisable()
        {
            _characterPanel.InspectingCharacter -= UpdateCharacterEquipments;
            _equipmentsInventoryPanel.UnequipPressed -= UnequipCurrentSlot;
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ShowEquipmentsInventoryWithType -= ShowEquipmentsInventoryWithType;
            }
        }


        private void UpdateCharacterEquipments(CharacterSpec charSpec)
        {
            _currentInspectingCharacter = charSpec;
            if (charSpec.IsValid() == false) return;

            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.RemoveCharacterEquipmentsEvents(_currentInspectingCharacter.Equipments);
                equipmentSlot.Init(_currentInspectingCharacter.Equipments.GetEquipmentInSlot(equipmentSlot.SlotType));
                equipmentSlot.RegisterCharacterEquipmentsEvents(_currentInspectingCharacter.Equipments);
            }
        }

        private EquipmentSlot.EType _modifyingSlotType;
        public EquipmentSlot.EType ModifyingSlotType => _modifyingSlotType;
        public EEquipmentCategory EquipmentCategory { get; private set; }

        private void UnequipCurrentSlot() => UnequipEquipmentAtSlot?.Invoke(_modifyingSlotType);

        private void ShowEquipmentsInventoryWithType(EquipmentSlot.EType slotType, EEquipmentCategory category)
        {
            HideToolTipAndDeselectCurrentSelectedButton();
            EquipmentCategory = category;
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
            if (button) button.Select();
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
            SetEquipmentsUI(_currentInspectingCharacter.Equipments);

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