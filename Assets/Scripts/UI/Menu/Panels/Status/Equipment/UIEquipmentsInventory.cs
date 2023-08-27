using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
using CryptoQuest.System;
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
        [Header("Configs")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private InventorySO _inventorySO; // TODO: refactor to use interface instead
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;

        [Header("Game Components")]
        [SerializeField] private UIEquipment _currentlyEquippingItem;
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private RectTransform _singleItemRect;

        [SerializeField] private ServiceProvider _serviceProvider;

        private float _verticalOffset;
        private bool _initialized;

        private RectTransform _inventoryViewport;
        private float _lowerBound;
        private float _upperBound;

        private List<EquipmentInfo> _equipments;
        private EEquipmentCategory _cachedType;

        private void Awake()
        {
            _verticalOffset = _singleItemRect.rect.height;
            _inventoryViewport = _scrollRect.viewport;
            var position = _inventoryViewport.position;
            var rect = _inventoryViewport.rect;
            _lowerBound = position.y - rect.height / 2;
            _upperBound = position.y + rect.height / 2 + _verticalOffset;

            InstantiateEquipments();
        }

        private void OnEnable()
        {
            _unEquipButton.onClick.AddListener(OnUnequip);
        }

        private void OnDisable()
        {
            _unEquipButton.onClick.RemoveListener(OnUnequip);
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

        public void Show(CharacterSpec inspectingChar, EquipmentSlot.EType modifyingSlotType)
        {
            _inspectingCharacter = inspectingChar;
            _inspectingCharacter.Equipments.EquipmentRemoved += RemoveCurrentlyEquipping;
            _scrollRect.content.anchoredPosition = Vector2.zero;
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            _contents.SetActive(true);
            _unEquipButton.Select();
            RenderCurrentlyEquipItem(inspectingChar, modifyingSlotType);
        }

        private void RenderCurrentlyEquipItem(CharacterSpec inspectingCharacter,
            EquipmentSlot.EType modifyingSlotType)
        {
            var equipment = inspectingCharacter.Equipments.GetEquipmentInSlot(modifyingSlotType);
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
            _inspectingCharacter.Equipments.EquipmentRemoved -= RemoveCurrentlyEquipping;
            _tooltipProvider.Tooltip.Hide();
            _contents.SetActive(false);
        }

        private List<UIEquipmentItem> _equipmentItems = new();

        /// <summary>
        /// An equipment just removed from the character equipment inventory, this could be the current equipping
        /// Add UI to the inventory scroll view and hide the currently equipping UI if the same
        /// </summary>
        /// <param name="equipment"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RemoveCurrentlyEquipping(EquipmentInfo equipment)
        {
            if (equipment != _currentlyEquippingItem.Equipment) return;
            _currentlyEquippingItem.Reset();
            _currentlyEquippingItem.gameObject.SetActive(false);
            InstantiateNewEquipmentUI(equipment);
        }

        private void InstantiateEquipments()
        {
            if (_initialized) return;
            _initialized = true;
            for (int i = 0; i < _inventorySO.Equipments.Count; i++)
            {
                var equipment = _inventorySO.Equipments[i];
                InstantiateNewEquipmentUI(equipment);
            }
        }

        private void InstantiateNewEquipmentUI(EquipmentInfo equipment)
        {
            var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
            equipmentItem.Init(equipment);
            equipmentItem.Inspecting += PreviewEquipmentStats;
        }

        private void OnDestroy()
        {
            foreach (var equipmentItem in _equipmentItems)
            {
                equipmentItem.Inspecting -= PreviewEquipmentStats;
                Destroy(equipmentItem.gameObject);
            }
        }

        private void PreviewEquipmentStats(EquipmentInfo equipmentToPreview) { }

        public void OnUnequip()
        {
            UnequipPressed?.Invoke();
        }
    }
}