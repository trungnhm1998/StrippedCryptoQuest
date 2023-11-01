using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.Menus.Status.States;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Show currently equipped equipments
    /// </summary>
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        // refs
        [SerializeField] private UIStatusMenu _mainPanel;
        [SerializeField] private UIStatusCharacter _characterPanel;
        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;

        // config
        [SerializeField] private GameObject _equipmentSlotParent;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;

        // Tooltips
        private ITooltip _tooltip;
        private ITooltip Tooltip => _tooltip ??= TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private Vector2 _minPivotTooltip = Vector2.zero;
        [SerializeField] private Vector2 _maxPivotTooltip = Vector2.one;

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

        private void Awake()
        {
            ResetEquipmentsUI();
        }

#if UNITY_EDITOR
        private void OnValidate() => _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
#endif

        private void OnEnable()
        {
            Tooltip.WithBoderPointer(true)
                .WithLocalPosition(Vector3.zero)
                .WithScale(Vector3.one)
                .WithRangePivot(_minPivotTooltip, _maxPivotTooltip);

            _characterPanel.InspectingCharacter += UpdateCharacterEquipments;
            foreach (var equipmentSlot in _equipmentSlots)
                equipmentSlot.ShowEquipmentsInventoryWithType += ShowEquipmentsInventoryWithType;
        }

        private void OnDisable()
        {
            _characterPanel.InspectingCharacter -= UpdateCharacterEquipments;
            foreach (var equipmentSlot in _equipmentSlots)
                equipmentSlot.ShowEquipmentsInventoryWithType -= ShowEquipmentsInventoryWithType;
        }

        public void Show()
        {
            // TODO: REFACTOR EQUIPMENTS
            SetEquipmentsUI(_characterPanel.InspectingHero);

            Tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentSlotParent.SetActive(true);
            _defaultSelection.Select();
        }

        public void Hide()
        {
            Tooltip.Hide();
            _equipmentSlotParent.SetActive(false);
        }

        private void UpdateCharacterEquipments(HeroBehaviour hero)
        {
            // _inspectingHero = hero;
            if (hero.IsValidAndAlive() == false) return;

            // TODO: REFACTOR EQUIPMENTS
            // foreach (var equipmentSlot in _equipmentSlots)
            // {
            //     equipmentSlot.RemoveCharacterEquipmentsEvents(_inspectingHero.Equipments);
            //     equipmentSlot.Init(_inspectingHero.Equipments.GetEquipmentInSlot(equipmentSlot.SlotType));
            //     equipmentSlot.RegisterCharacterEquipmentsEvents(_inspectingHero.Equipments);
            // }
        }

        public EquipmentSlot.EType EquippingSlot { get; private set; }

        public EEquipmentCategory ModifyingEquipmentCategory { get; private set; }

        private void ShowEquipmentsInventoryWithType(EquipmentSlot.EType slotType, EEquipmentCategory category)
        {
            HideToolTipAndDeselectCurrentSelectedButton();
            ModifyingEquipmentCategory = category;
            EquippingSlot = slotType;
            _mainPanel.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }

        private void SetEquipmentsUI(HeroBehaviour hero)
        {
            var equipments = hero.GetComponent<EquipmentsController>().Equipments;
            ResetEquipmentsUI();
            for (int i = 0; i < equipments.Slots.Count; i++)
            {
                var equipmentSlot = equipments.Slots[i];
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
            Tooltip.Hide();
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
    }
}