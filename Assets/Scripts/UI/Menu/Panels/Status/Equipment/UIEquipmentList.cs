using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentList : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private InventorySO _inventorySO;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        [SerializeField] private MultiInputButton _unEquipButton;
        [SerializeField] private RectTransform _singleItemRect;

        private float _verticalOffset;
        private bool _initialized = false;

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
        }

        private void OnEnable()
        {
            UIEquipmentButton.InspectingRow += AutoScroll;
            _inputMediator.MenuNavigationContextEvent += NavigateMenu;
        }

        private void OnDisable()
        {
            UIEquipmentButton.InspectingRow -= AutoScroll;
            _inputMediator.MenuNavigationContextEvent -= NavigateMenu;
        }

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

        public void Show(EEquipmentCategory type)
        {
            _contents.SetActive(true);
            _unEquipButton.Select();
            LoadItems(type);
        }

        private void RefreshItems()
        {
            for (int i = 1; i < _scrollRect.content.childCount; i++)
            {
                Destroy(_scrollRect.content.GetChild(i).gameObject);
            }
        }

        private void LoadItems(EEquipmentCategory type)
        {
            if (type != _cachedType)
            {
                RefreshItems();
                _initialized = false;
            }

            _cachedType = type;

            if (!_initialized)
            {
                _scrollRect.Initialize(this);
                _initialized = true;
            }

            if (!_inventorySO.GetEquipmentByType(type, out _equipments))
            {
                Debug.LogWarning("Failed to get equipments");
            }
        }

        public void Hide()
        {
            _unEquipButton.Select();
            _contents.SetActive(false);
            UITooltip.HideTooltipEvent?.Invoke();
        }

        #region PLUGINS

        /// <summary>
        /// needed for plugins
        /// </summary>
        /// <returns>Real data count</returns>
        public int GetItemCount()
        {
            return _equipments.Count;
        }

        /// <summary>
        /// Will be called auto
        /// </summary>
        /// <param name="cell">The prefab that we can cast to set Data to UI</param>
        /// <param name="index">query from real data using this index</param>
        public void SetCell(ICell cell, int index)
        {
            // UIEquipmentItem itemRow = cell as UIEquipmentItem;
            // itemRow.Init(_equipments[index], index);
            // itemRow.interactable = true;
        }

        #endregion

        public void Init(IParty party) { }
    }
}