using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Show currently equipped equipments
    /// </summary>
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        // refs
        [SerializeField] private UIStatusMenu _mainPanel;
        [SerializeField] private UIEquipmentsInventory _equipmentsInventoryPanel;
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;

        // config
        [SerializeField] private GameObject _equipmentSlotParent;
        [SerializeField] private MultiInputButton _defaultSelection;

        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private Vector2 _minPivotTooltip = Vector2.zero;
        [SerializeField] private Vector2 _maxPivotTooltip = Vector2.one;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> _equipmentSlotsCache = new();
        public Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> EquipmentSlots
        {
            get
            {
                if (_equipmentSlotsCache != null && _equipmentSlotsCache.Count != 0) return _equipmentSlotsCache;
                _equipmentSlotsCache = new Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot>();
                foreach (var equipmentSlot in _equipmentSlots)
                    _equipmentSlotsCache.Add(equipmentSlot.SlotType, equipmentSlot);

                return _equipmentSlotsCache;
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
#endif

        private void OnEnable()
        {
            // TODO: REFACTOR TOOLTIP
            // Tooltip.WithBorderPointer(true)
            //     .WithLocalPosition(Vector3.zero)
            //     .WithScale(Vector3.one)
            //     .WithRangePivot(_minPivotTooltip, _maxPivotTooltip);
        }

        public void Show(HeroBehaviour hero)
        {
            RenderEquippingItems(hero);
            // TODO: REFACTOR TOOLTIP
            // Tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentSlotParent.SetActive(true);
            Invoke(nameof(SelectDefault), 0);
        }

        public void SelectDefault() => _defaultSelection.Select();

        public void Hide()
        {
            // TODO: REFACTOR TOOLTIP
            // Tooltip.Hide();
            _equipmentSlotParent.SetActive(false);
        }

        private void RenderEquippingItems(HeroBehaviour hero)

        {
            if (hero.IsValid() == false) return;
            var equipments = hero.GetComponent<EquipmentsController>().Equipments;
            ResetEquipmentsUI();
            foreach (var equipmentSlot in equipments.Slots)
            {
                if (equipmentSlot.IsValid() == false) continue;
                var uiEquipmentSlot = EquipmentSlots[equipmentSlot.Type];
                uiEquipmentSlot.Init(equipmentSlot.Equipment);
            }

            // TODO: REFACTOR TOOLTIP
            // Tooltip.Hide();
        }

        private void ResetEquipmentsUI()
        {
            foreach (var equipmentSlot in _equipmentSlots) equipmentSlot.Reset();
        }
    }
}