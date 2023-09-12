using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    /// <summary>
    /// Show all the equipments in the inventory
    /// </summary>
    public class UIEquipmentsInventory : MonoBehaviour
    {
        public event Action UnequipPressed;
        public event Action<EquipmentInfo, CharacterSpec> InspectingEquipment;
        [SerializeField] private UIStatusMenu _main;

        [Header("Configs")]
        [SerializeField] private ScrollRect _scrollRect;

        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private RectTransform _singleItemRect;
        [SerializeField] private ServiceProvider _serviceProvider;
        [SerializeField] private UIEquipmentPreviewer _equipmentPreviewer;

        private float _verticalOffset;
        private RectTransform _inventoryViewport;
        private float _lowerBound;
        private float _upperBound;
        private List<UIEquipmentItem> _equipmentItems = new();

        private void Awake()
        {
            _verticalOffset = _singleItemRect.rect.height;
            _inventoryViewport = _scrollRect.viewport;
            var rect = _inventoryViewport.rect;
            _lowerBound = _verticalOffset * 2;
            _upperBound = rect.height;
        }

        private void OnEnable()
        {
            _unEquipButton.onClick.AddListener(OnUnequip);
            _unEquipButton.Selected += PreviewUnselectEquipment;
        }

        private void OnDisable()
        {
            _unEquipButton.onClick.RemoveListener(OnUnequip);
            _unEquipButton.Selected -= PreviewUnselectEquipment;
        }

        /// <summary>
        /// Reset to position to top of the list
        /// </summary>
        /// <param name="context"></param>
        private void NavigateMenu(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject.name == _unEquipButton.gameObject.name)
            {
                _scrollRect.content.anchoredPosition = Vector2.zero;
            }
        }

        private void AutoScroll(Button button)
        {
            var selectedRowPositionY = button.transform.position.y;

            if (selectedRowPositionY <= _lowerBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.up * _verticalOffset;
            }
            else if (selectedRowPositionY >= _upperBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.down * _verticalOffset;
            }
        }

        private void AlignItemRow(float selectedRowPositionY, float lowerBound)
        {
            if (selectedRowPositionY <= lowerBound + _verticalOffset)
            {
                var diff = (lowerBound + _verticalOffset) - selectedRowPositionY;
                _scrollRect.content.anchoredPosition += Vector2.up * diff;
            }
        }

        private CharacterSpec _inspectingCharacter;
        private EquipmentSlot.EType _slotType;
        private EEquipmentCategory _category;

        public void Show(CharacterSpec inspectingChar, EquipmentSlot.EType modifyingSlotType,
            EEquipmentCategory category)
        {
            _category = category;
            Reset();
            _slotType = modifyingSlotType;
            _inspectingCharacter = inspectingChar;
            _inspectingCharacter.Equipments.EquipmentAdded += UpdateInventoryAndEquippingUI;
            _inspectingCharacter.Equipments.EquipmentRemoved += RemoveCurrentlyEquipping;
            _scrollRect.content.anchoredPosition = Vector2.zero;
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            _contents.SetActive(true);
            _unEquipButton.Select();
            InstantiateEquipments();
            RenderCurrentlyEquipItem(inspectingChar, modifyingSlotType);
            PreviewUnselectEquipment();
        }

        private void RenderCurrentlyEquipItem(CharacterSpec inspectingCharacter,
            EquipmentSlot.EType modifyingSlotType)
        {
            var equipment = inspectingCharacter.Equipments.GetEquipmentInSlot(modifyingSlotType);
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
            _inspectingCharacter.Equipments.EquipmentAdded -= UpdateInventoryAndEquippingUI;
            _inspectingCharacter.Equipments.EquipmentRemoved -= RemoveCurrentlyEquipping;
            _tooltipProvider.Tooltip.Hide();
            _contents.SetActive(false);
            Reset();
            _equipmentPreviewer.ResetAttributesUI();
        }

        private void Reset()
        {
            for (var index = 0; index < _equipmentItems.Count; index++)
            {
                var equipmentItem = _equipmentItems[index];
                DestroyEquipmentRow(equipmentItem);
            }

            _equipmentItems.Clear();
        }

        private void DestroyEquipmentRow(UIEquipmentItem equipmentItem)
        {
            equipmentItem.EquipItem -= EquipEquipment;
            equipmentItem.Inspecting -= OnPreviewEquipmentStats;
            Destroy(equipmentItem.gameObject);
        }

        private void UpdateInventoryAndEquippingUI(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (equipment.IsValid() == false) return;
            bool contains = false;
            foreach (var slot in equipment.RequiredSlots)
            {
                if (slot == _slotType)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains) return;
            _tooltipProvider.Tooltip.Hide();
            RemoveEquipmentFromInventory(equipment);
            UpdateCurrentlyEquipping(equipment);
            PreviewUnselectEquipment();
        }

        private void RemoveEquipmentFromInventory(EquipmentInfo equipment)
        {
            UIEquipmentItem equipmentItem = null;
            var index = 0;
            for (; index < _equipmentItems.Count; index++)
            {
                var item = _equipmentItems[index];
                if (item.Equipment.IsValid() && item.Equipment == equipment)
                {
                    equipmentItem = item;
                    Debug.Log($"item {item} idx: {index}");
                    break;
                }
            }

            if (equipmentItem == null) return;
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log($"RemoveEquipmentFromInventory {equipmentItem} idx: {index}");
            _serviceProvider.InventoryController.Remove(equipment);
            _equipmentItems.RemoveAt(index);
            DestroyEquipmentRow(equipmentItem);
            EventSystem.current.SetSelectedGameObject(_unEquipButton.gameObject);
        }

        /// <summary>
        /// An equipment just removed from the character equipment inventory, this could be the current equipping
        /// Add UI to the inventory scroll view and hide the currently equipping UI if the same
        /// </summary>
        /// <param name="equipment"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RemoveCurrentlyEquipping(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (equipment.IsValid() && equipment != _currentlyEquippingItem.Equipment) return;
            _currentlyEquippingItem.gameObject.SetActive(false);
            _currentlyEquippingItem.Reset();
            _serviceProvider.InventoryController.Add(equipment);
            InstantiateNewEquipmentUI(equipment);
        }

        private void InstantiateEquipments()
        {
            _equipmentItems.Clear();
            _equipmentItems = new();
            for (int i = 0; i < _serviceProvider.Inventory.Equipments.Count; i++)
            {
                var equipment = _serviceProvider.Inventory.Equipments[i];
                if (_category == equipment.Data.EquipmentCategory)
                    InstantiateNewEquipmentUI(equipment);
            }

            Debug.Log($"InstantiateEquipments {_equipmentItems.Count}");
        }

        private void InstantiateNewEquipmentUI(EquipmentInfo equipment)
        {
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);

            equipmentItem.Inspecting += OnPreviewEquipmentStats;
            equipmentItem.EquipItem += EquipEquipment;
            _equipmentItems.Add(equipmentItem);

            ValidateEquipment(equipment, equipmentItem);
        }


        private void ValidateEquipment(EquipmentInfo equipment, UIEquipmentItem equipmentItem)
        {
            if (equipment.IsCompatibleWithCharacter(_inspectingCharacter)) return;

            equipmentItem.DeactivateButton();
        }

        private void EquipEquipment(EquipmentInfo equipment)
        {
            _main.EquipItem(equipment);
            _main.State.RequestStateChange(StatusMenuStateMachine.Status);
        }

        private GameObject _cloneChar;

        private void PreviewUnselectEquipment()
        {
            OnPreviewEquipmentStats(_currentlyEquippingItem.Equipment);
        }

        private void OnPreviewEquipmentStats(EquipmentInfo equipment)
        {
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            InspectingEquipment?.Invoke(equipment, _inspectingCharacter);
            GetEquipmentSelected(equipment);
        }

        private void GetEquipmentSelected(EquipmentInfo equipment)
        {
            var item = _equipmentItems.FirstOrDefault(item => item.Equipment == equipment);
            if (item != null)
            {
                AutoScroll(item.GetComponent<MultiInputButton>());
            }
        }

        public void OnUnequip()
        {
            _equipmentPreviewer.ResetAttributesUI();
            UnequipPressed?.Invoke();
        }
    }
}