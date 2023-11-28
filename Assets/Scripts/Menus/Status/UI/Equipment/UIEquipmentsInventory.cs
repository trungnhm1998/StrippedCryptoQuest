using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
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
        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private UIEquipmentPreviewer _equipmentPreviewer;

        private List<UIEquipmentItem> _equipmentItems = new();
        private EquipmentsController _equipmentsController;

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

            _prefabDatabase.ReleaseAllData();
        }

        public void RenderEquipmentsInInventory(HeroBehaviour hero, EquipmentSlot.EType slotType,
            EEquipmentCategory categoryType)
        {
            _contents.SetActive(true);
            _hero = hero;
            _slotType = slotType;
            _categoryType = categoryType;
            _equipmentsController = hero.GetComponent<EquipmentsController>();
            Reset();
            RegisterEquippingEvents();
            InstantiateEquipments();
            RenderCurrentlyEquipItem();
            Invoke(nameof(SelectUnequipButton), 0);
        }

        private void SelectUnequipButton() => _unEquipButton.Select();

        private void Reset()
        {
            _scrollRect.content.anchoredPosition = Vector2.zero;
            // TODO: REFACTOR TOOLTIP
            // _tooltip.SetSafeArea(_tooltipSafeArea);
            UnregisterEquippingEvents();
            foreach (var equipmentItem in _equipmentItems) DestroyEquipmentRow(equipmentItem);
            _equipmentItems.Clear();
        }

        private void RegisterEquippingEvents()
        {
            if (_equipmentsController == null) return;
            _equipmentsController.Removed += RemoveCurrentlyEquipping;
            _equipmentsController.Equipped += UpdateInventoryAndEquippingUI;
        }

        private void UnregisterEquippingEvents()
        {
            if (_equipmentsController == null) return;
            _equipmentsController.Removed -= RemoveCurrentlyEquipping;
            _equipmentsController.Equipped -= UpdateInventoryAndEquippingUI;
        }

        private void RenderCurrentlyEquipItem()
        {
            var equipmentController = _equipmentsController;
            var equipment = equipmentController.GetEquipmentInSlot(_slotType);
            UpdateCurrentlyEquipping(equipment);
        }

        private void UpdateCurrentlyEquipping(EquipmentInfo equipment)
        {
            var isValid = equipment != null && equipment.IsValid();
            _currentlyEquippingItem.gameObject.SetActive(isValid);
            if (isValid == false)
            {
                _currentlyEquippingItem.Reset();
                return;
            }

            _currentlyEquippingItem.Init(equipment);
        }

        public void Hide()
        {
            Reset();
            // TODO: REFACTOR TOOLTIP
            // _tooltip.Hide();
            _contents.SetActive(false);
            _equipmentPreviewer.ResetAttributesUI();
        }

        private void DestroyEquipmentRow(UIEquipmentItem equipmentItem)
        {
            if (equipmentItem == null) return;
            equipmentItem.Deselected -= ResetPreviewer;
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

            // TODO: REFACTOR TOOLTIP
            // _tooltip.Hide();

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
        private void RemoveCurrentlyEquipping(EquipmentInfo equipment)
        {
            if (equipment.IsValid() && equipment != _currentlyEquippingItem.Equipment) return;
            _currentlyEquippingItem.gameObject.SetActive(false);
            _currentlyEquippingItem.Reset();

            StartCoroutine(InstantiateNewEquipmentUICo(equipment));
        }

        private void InstantiateEquipments()
        {
            _equipmentItems.Clear();
            _equipmentItems = new();
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;

            foreach (var equipment in inventory.Equipments) StartCoroutine(InstantiateNewEquipmentUICo(equipment));
            foreach (var equipment in inventory.NftEquipments) StartCoroutine(InstantiateNewEquipmentUICo(equipment));
        }

        private IEnumerator InstantiateNewEquipmentUICo(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false ||
                _prefabDatabase.CacheLookupTable.ContainsKey(equipment.Data.PrefabId) == false)
            {
                Debug.Log(
                    $"UIEquipmentsInventory::InstantiateNewEquipmentUICo: Equipment [{equipment}] is not valid");
                yield break;
            }

            yield return _prefabDatabase.LoadDataById(equipment.Data.PrefabId);
            var prefab = _prefabDatabase.GetDataById(equipment.Data.PrefabId);

            if (_categoryType != prefab.EquipmentCategory) yield break;
            if (prefab.AllowedSlots.Contains(_slotType) == false) yield break;
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);
            equipmentItem.Deselected += ResetPreviewer;
            equipmentItem.Inspecting += OnPreviewEquipmentStats;
            equipmentItem.EquipItem += EquipEquipment;
            _equipmentItems.Add(equipmentItem);

            _hero.TryGetComponent(out LevelSystem levelSystem);
            var equipmentAllowedClasses = prefab.EquipmentType.AllowedClasses;
            if (equipment.Data.RequiredCharacterLevel > levelSystem.Level)
            {
                Debug.LogWarning("Character level is not enough");
                equipmentItem.DeactivateButton();
                yield break;
            }

            if (!Array.Exists(equipmentAllowedClasses, allowedClass => allowedClass == _hero.Class))
            {
                Debug.LogWarning("Character class is not allowed");
                equipmentItem.DeactivateButton();
            }
        }

        private void ResetPreviewer(UIEquipmentItem _)
        {
            _equipmentPreviewer.ResetAttributesUI();
        }

        private void OnPreviewEquipmentStats(UIEquipmentItem equippingItemUI)
        {
            UnregisterEquippingEvents();

            // TODO: REFACTOR TOOLTIP
            // _tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentPreviewer.PreviewEquipment(equippingItemUI.Equipment, _slotType, _hero);

            RegisterEquippingEvents();
        }

        private UIEquipmentItem _equippingItemToBeRemoveFromInventory;
        private EquipmentSlot.EType _slotType;
        private EEquipmentCategory _categoryType;
        private HeroBehaviour _hero;

        private void EquipEquipment(UIEquipmentItem equippingItemUI)
        {
            _equippingItemToBeRemoveFromInventory = equippingItemUI;
            _equipmentsController.Equip(equippingItemUI.Equipment, _slotType);
        }

        private void PreviewUnselectEquipment()
        {
            if (_currentlyEquippingItem.Equipment == null || !_currentlyEquippingItem.Equipment.IsValid())
            {
                _equipmentPreviewer.ResetAttributesUI();
                return;
            }

            // Since previewer try to equip/unequip I have to remove event so that wont affect UI
            UnregisterEquippingEvents();

            // TODO: REFACTOR TOOLTIP
            // _tooltip.SetSafeArea(_tooltipSafeArea);
            _equipmentPreviewer.PreviewUnequipEquipment(_currentlyEquippingItem.Equipment, _slotType, _hero);

            RegisterEquippingEvents();
        }

        private void Unequip()
        {
            var equipmentsController = _equipmentsController;
            _equipmentPreviewer.ResetAttributesUI();
            equipmentsController.Unequip(_slotType);
        }
    }
}