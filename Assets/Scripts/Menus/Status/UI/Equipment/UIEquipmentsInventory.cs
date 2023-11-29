using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Show all the equipments in the inventory
    /// </summary>
    public class UIEquipmentsInventory : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private GameObject _contents;

        private List<UIEquipmentItem> _equipmentItems = new();
        private EquipmentsController _equipmentsController;

        private void RegisterEquippingEvents()
        {
            _equipmentsController.Removed += RemoveCurrentlyEquipping;
            _equipmentsController.Equipped += RefreshInventoryListAndEquippingUI;
        }

        private void UnregisterEquippingEvents()
        {
            _equipmentsController.Removed -= RemoveCurrentlyEquipping;
            _equipmentsController.Equipped -= RefreshInventoryListAndEquippingUI;
        }

        private void RefreshInventoryListAndEquippingUI(IEquipment equipment)
        {
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

            DestroyEquipmentRow(_equippingItemToBeRemoveFromInventory);
            _equippingItemToBeRemoveFromInventory = null;
            UpdateCurrentlyEquipping(equipment);
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
        }

        private void Reset()
        {
            _scrollRect.content.anchoredPosition = Vector2.zero;
            UnregisterEquippingEvents();
            foreach (var equipmentItem in _equipmentItems) DestroyEquipmentRow(equipmentItem);
            _equipmentItems.Clear();
        }

        private void RenderCurrentlyEquipItem()
        {
            var equipmentController = _equipmentsController;
            var equipment = equipmentController.GetEquipmentInSlot(_slotType);
            UpdateCurrentlyEquipping(equipment);
        }

        private void UpdateCurrentlyEquipping(IEquipment equipment)
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
            _contents.SetActive(false);
        }

        private void DestroyEquipmentRow(UIEquipmentItem equipmentItem)
        {
            if (equipmentItem == null) return;
            equipmentItem.EquipItem -= EquipEquipment;
            Destroy(equipmentItem.gameObject);
        }

        /// <summary>
        /// An equipment just removed from the character equipment inventory, this could be the current equipping
        /// Add UI to the inventory scroll view and hide the currently equipping UI if the same
        /// </summary>
        /// <param name="equipment"></param>
        private void RemoveCurrentlyEquipping(IEquipment equipment)
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

        private IEnumerator InstantiateNewEquipmentUICo(IEquipment equipment)
        {
            if (equipment == null || equipment.IsValid() == false) yield break;
            var prefab = equipment.Prefab;
            if (_categoryType != prefab.EquipmentCategory) yield break;
            if (prefab.AllowedSlots.Contains(_slotType) == false) yield break;
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);
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

        private UIEquipmentItem _equippingItemToBeRemoveFromInventory;
        private EquipmentSlot.EType _slotType;
        private EEquipmentCategory _categoryType;
        private HeroBehaviour _hero;

        private void EquipEquipment(UIEquipmentItem equippingItemUI)
        {
            _equippingItemToBeRemoveFromInventory = equippingItemUI;
            _equipmentsController.Equip(equippingItemUI.Equipment, _slotType);
        }

        public void UnequipPressed() => _equipmentsController.Unequip(_slotType);
    }
}