using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusInventory : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;

        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private InventorySO _inventorySO;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;

        [SerializeField] private GameObject _unEquipButton;
        [SerializeField] private RectTransform _singleItemRect;

        private float _verticalOffset;
        private bool _initialized = false;

        private RectTransform _inventoryViewport;
        private float _lowerBound;
        private float _upperBound;

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
            UIStatusInventoryItemButton.InspectingRow += AutoScroll;
            _inputMediator.MenuNavigationContextEvent += NavigateMenu;
        }

        private void OnDisable()
        {
            UIStatusInventoryItemButton.InspectingRow -= AutoScroll;
            _inputMediator.MenuNavigationContextEvent -= NavigateMenu;
        }

        private void NavigateMenu(InputAction.CallbackContext context)
        {
            if (EventSystem.current.currentSelectedGameObject.name == _unEquipButton.name)
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

            AlignItemRow(selectedRowPositionY, _lowerBound);
        }

        private void AlignItemRow(float selectedRowPositionY, float lowerBound)
        {
            if (selectedRowPositionY <= lowerBound + _verticalOffset)
            {
                var diff = (lowerBound + _verticalOffset) - selectedRowPositionY;
                _scrollRect.content.anchoredPosition += Vector2.up * diff;
            }
        }

        public void Show(UIEquipmentSlotButton.EEquipmentType statusPanelEquippingType)
        {
            _contents.SetActive(true);

            // only init after get data
            if (!_initialized)
            {
                _initialized = true;
                _scrollRect.Initialize(this);
            }
        }

        public void Hide()
        {
            _contents.SetActive(false);
        }

        #region PLUGINS

        /// <summary>
        /// needed for plugins
        /// </summary>
        /// <returns>Real data count</returns>
        public int GetItemCount()
        {
            return _inventorySO.Equipments.Count;
        }

        /// <summary>
        /// Will be called auto
        /// </summary>
        /// <param name="cell">The prefab that we can cast to set Data to UI</param>
        /// <param name="index">query from real data using this index</param>
        public void SetCell(ICell cell, int index)
        {
            UIStatusInventoryItemButton itemButtonRow = cell as UIStatusInventoryItemButton;
            itemButtonRow.Init(_inventorySO.Equipments[index], index);
        }

        #endregion
    }
}