using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.System;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Show all the equipments in the inventory
    /// </summary>
    public class UIEquipmentsInventory : MonoBehaviour
    {
        [SerializeField] private UIStatusCharacter _characterPanel;
        [SerializeField] private UICharacterEquipmentsPanel _equipmentsPanel;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private UIEquipmentPreviewer _equipmentPreviewer;

        private List<UIEquipmentItem> _equipmentItems = new();
        private HeroBehaviour InspectingHero => _characterPanel.InspectingHero;
        private EquipmentsController _equipmentsController;
        private ITooltip _tooltip;

        private void OnEnable()
        {
            _unEquipButton.onClick.AddListener(Unequip);
            _unEquipButton.Selected += PreviewUnselectEquipment;
        }

        private void OnDisable()
        {
            Reset();
            _unEquipButton.onClick.RemoveListener(Unequip);
            _unEquipButton.Selected -= PreviewUnselectEquipment;
        }

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
        }

        public void Show()
        {
            Reset();
            _equipmentsController = InspectingHero.GetComponent<EquipmentsController>();
            RegistEquippingEvents();
            _scrollRect.content.anchoredPosition = Vector2.zero;
            _tooltip.SetSafeArea(_tooltipSafeArea);
            _contents.SetActive(true);
            InstantiateEquipments();
            RenderCurrentlyEquipItem();
            _unEquipButton.Select();
            PreviewUnselectEquipment();
        }

        private void RegistEquippingEvents()
        {
            if (_equipmentsController == null) return;
            _equipmentsController.Removed += RemoveCurrentlyEquipping;
            _equipmentsController.Equipped += UpdateInventoryAndEquippingUI;
        }

        private void RemoveRegistEquippingEvents()
        {
            if (_equipmentsController == null) return;
            _equipmentsController.Removed -= RemoveCurrentlyEquipping;
            _equipmentsController.Equipped -= UpdateInventoryAndEquippingUI;
        }

        private void RenderCurrentlyEquipItem()
        {
            var equipmentController = _equipmentsController;
            var equipment = equipmentController.GetEquipmentInSlot(_equipmentsPanel.EquippingSlot);
            UpdateCurrentlyEquipping(equipment);
        }

        private void UpdateCurrentlyEquipping(EquipmentInfo equipment)
        {
            _currentlyEquippingItem.gameObject.SetActive(equipment.IsValid());
            if (!equipment.IsValid())
            {
                _currentlyEquippingItem.Reset();
                return;
            }

            _currentlyEquippingItem.Init(equipment);
        }

        public void Hide()
        {
            Reset();
            _tooltip.Hide();
            _contents.SetActive(false);
            _equipmentPreviewer.ResetAttributesUI();
        }

        private void Reset()
        {
            RemoveRegistEquippingEvents();

            for (var index = 0; index < _equipmentItems.Count; index++)
            {
                var equipmentItem = _equipmentItems[index];
                DestroyEquipmentRow(equipmentItem);
            }

            _equipmentItems.Clear();
        }

        private void DestroyEquipmentRow(UIEquipmentItem equipmentItem)
        {
            if (equipmentItem == null) return;
            equipmentItem.EquipItem -= EquipEquipment;
            equipmentItem.Inspecting -= OnPreviewEquipmentStats;
            Destroy(equipmentItem.gameObject);
        }

        private void UpdateInventoryAndEquippingUI(EquipmentInfo equipment)
        {
            if (equipment == null || equipment.IsValid() == false) return;
            if (_equippingItemToBeRemoveFromInventory == null)
            {
                Debug.LogWarning($"Equipped item into character were raised but last interacted UI is null" +
                                 $"\nThis likely because the UIEquipmentItem::EquipItem event raised twice");
                return;
            }

            if (equipment != _equippingItemToBeRemoveFromInventory.Equipment)
            {
                Debug.LogWarning("Equipped item into character were raised but last interacted UI is not the same");
                return;
            }

            _tooltip.Hide();

            EventSystem.current.SetSelectedGameObject(null);
            DestroyEquipmentRow(_equippingItemToBeRemoveFromInventory);
            _equippingItemToBeRemoveFromInventory = null;
            EventSystem.current.SetSelectedGameObject(_unEquipButton.gameObject);
            UpdateCurrentlyEquipping(equipment);
            PreviewUnselectEquipment();
        }

        /// <summary>
        /// An equipment just removed from the character equipment inventory, this could be the current equipping
        /// Add UI to the inventory scroll view and hide the currently equipping UI if the same
        /// </summary>
        /// <param name="equipment"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RemoveCurrentlyEquipping(EquipmentInfo equipment)
        {
            if (equipment.IsValid() && equipment != _currentlyEquippingItem.Equipment) return;
            _currentlyEquippingItem.gameObject.SetActive(false);
            _currentlyEquippingItem.Reset();

            InstantiateNewEquipmentUI(equipment);
        }

        private void InstantiateEquipments()
        {
            _equipmentItems.Clear();
            _equipmentItems = new();
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            for (int i = 0; i < inventory.Equipments.Count; i++)
            {
                var equipment = inventory.Equipments[i];
                if (_equipmentsPanel.ModifyingEquipmentCategory == equipment.Data.EquipmentCategory)
                    InstantiateNewEquipmentUI(equipment);
            }

            Debug.Log($"InstantiateEquipments {_equipmentItems.Count}");
        }

        private void InstantiateNewEquipmentUI(EquipmentInfo equipment)
        {
            if (equipment.AllowedSlots.Contains(_equipmentsPanel.EquippingSlot) == false) return;
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);

            equipmentItem.Inspecting += OnPreviewEquipmentStats;
            equipmentItem.EquipItem += EquipEquipment;
            _equipmentItems.Add(equipmentItem);

            ValidateEquipment(equipment, equipmentItem);
        }

        private void ValidateEquipment(EquipmentInfo equipment, UIEquipmentItem equipmentItem)
        {
            if (equipment.IsCompatibleWithHero(InspectingHero)) return;

            equipmentItem.DeactivateButton();
        }

        private UIEquipmentItem _equippingItemToBeRemoveFromInventory;

        private void EquipEquipment(UIEquipmentItem equippingItemUI)
        {
            _equippingItemToBeRemoveFromInventory = equippingItemUI;
            _equipmentsController.Equip(equippingItemUI.Equipment, _equipmentsPanel.EquippingSlot);
        }

        private void PreviewUnselectEquipment()
        {
            // Since previewer try to equip/unequip I have to remove event so that wont affect UI
            RemoveRegistEquippingEvents();

            if (!_currentlyEquippingItem.Equipment.IsValid())
            {
                _equipmentPreviewer.ResetAttributesUI();
                return;
            }

            _tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentPreviewer.PreviewUnequipEquipment(_currentlyEquippingItem.Equipment,
                _equipmentsPanel.EquippingSlot, InspectingHero);

            RegistEquippingEvents();
        }

        private void OnPreviewEquipmentStats(UIEquipmentItem equippingItemUI)
        {
            RemoveRegistEquippingEvents();

            _tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentPreviewer.PreviewEquipment(equippingItemUI.Equipment, _equipmentsPanel.EquippingSlot,
                InspectingHero);

            RegistEquippingEvents();
        }

        private void Unequip()
        {
            var equipmentsController = _equipmentsController;
            _equipmentPreviewer.ResetAttributesUI();
            equipmentsController.Unequip(_equipmentsPanel.EquippingSlot);
        }
    }
}