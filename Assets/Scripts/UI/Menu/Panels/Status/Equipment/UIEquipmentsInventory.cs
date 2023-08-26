using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
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
        [Header("Configs")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private InventorySO _inventorySO; // TODO: refactor to use interface instead
        [SerializeField] private UIEquipmentItem _equipmentItemPrefab;

        [Header("Game Components")]
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private RectTransform _tooltipSafeArea;
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private RectTransform _singleItemRect;

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
            UIEquipmentButton.InspectingRow += AutoScroll;
        }

        private void OnDisable()
        {
            UIEquipmentButton.InspectingRow -= AutoScroll;
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

        public void Show()
        {
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            _contents.SetActive(true);
            _unEquipButton.Select();
            // LoadItems(type);
        }

        public void Hide()
        {
            _tooltipProvider.Tooltip.Hide();
            _contents.SetActive(false);
        }

        private void InstantiateEquipments()
        {
            if (_initialized) return;
            _initialized = true;
            for (int i = 0; i < _inventorySO.Equipments.Count; i++)
            {
                var equipment = _inventorySO.Equipments[i];
                var equipmentItem = Instantiate(_equipmentItemPrefab, _scrollRect.content);
                equipmentItem.Init(equipment);
            }
        }

        // private void RefreshItems()
        // {
        //     for (int i = 1; i < _scrollRect.content.childCount; i++)
        //     {
        //         Destroy(_scrollRect.content.GetChild(i).gameObject);
        //     }
        // }

        // private void LoadItems(EEquipmentCategory type)
        // {
        //     if (type != _cachedType)
        //     {
        //         RefreshItems();
        //         _initialized = false;
        //     }
        //
        //     _cachedType = type;
        //
        //     if (!_initialized)
        //     {
        //         _scrollRect.Initialize(this);
        //         _initialized = true;
        //     }
        //
        //     if (!_inventorySO.GetEquipmentByType(type, out _equipments))
        //     {
        //         Debug.LogWarning("Failed to get equipments");
        //     }
        // }
        //
        // public void Hide()
        // {
        //     _unEquipButton.Select();
        //     _contents.SetActive(false);
        //     // TODO: REFACTOR TOOL TIP HIDE
        // }
        //
        // public void Init(IParty party) { }
    }
}