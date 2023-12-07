using System;
using System.Collections;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Utilities;
using IndiGames.Core.Common;
using UI.Common;
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
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private GameObject _contents;

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

            var index = _equippingItemToBeRemoveFromInventory.transform.GetSiblingIndex();
            DestroyEquipmentRow(_equippingItemToBeRemoveFromInventory);
            _equippingItemToBeRemoveFromInventory = null;
            UpdateCurrentlyEquipping(equipment);

            var nextElement = index + 1;
            if (nextElement >= _scrollRect.content.childCount) nextElement = index - 1;
            EventSystem.current.SetSelectedGameObject(_scrollRect.content.GetChild(Math.Max(0, nextElement)).gameObject);
        }

        public void RenderEquipmentsInInventory(HeroBehaviour hero, ESlot modifySlot,
            EEquipmentCategory categoryType)
        {
            _contents.SetActive(true);
            if (_hero == hero && _slotSlot == modifySlot && _categoryType == categoryType) return;
            _hero = hero;
            _slotSlot = modifySlot;
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
            var children = _scrollRect.content.GetComponentsInChildren<UIEquipmentItem>();
            foreach (var child in children) DestroyEquipmentRow(child);
        }

        private void RenderCurrentlyEquipItem()
        {
            var equipmentController = _equipmentsController;
            var equipment = equipmentController.GetEquipmentInSlot(_slotSlot);
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
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;

            foreach (var equipment in inventory.Equipments) StartCoroutine(InstantiateNewEquipmentUICo(equipment));
            foreach (var equipment in inventory.NftEquipments) StartCoroutine(InstantiateNewEquipmentUICo(equipment));
        }

        private IEnumerator InstantiateNewEquipmentUICo(IEquipment equipment)
        {
            if (equipment == null || equipment.IsValid() == false) yield break;
            var prefab = equipment.Prefab;
            if (_categoryType != prefab.EquipmentCategory) yield break;
            if (prefab.AllowedSlots.Contains(_slotSlot) == false) yield break;
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);
            equipmentItem.EquipItem += EquipEquipment;

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
        private ESlot _slotSlot;
        private EEquipmentCategory _categoryType;
        private HeroBehaviour _hero;

        private void EquipEquipment(UIEquipmentItem equippingItemUI)
        {
            _equippingItemToBeRemoveFromInventory = equippingItemUI;
            _equipmentsController.Equip(equippingItemUI.Equipment, _slotSlot);
        }

        public void UnequipPressed() => _equipmentsController.Unequip(_slotSlot);
    }
}