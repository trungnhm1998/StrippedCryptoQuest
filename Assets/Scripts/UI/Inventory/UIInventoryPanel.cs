using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace CryptoQuest.UI.Inventory
{
    // TODO: rename class
    public class UIInventoryPanel : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _upHint;
        [SerializeField] private GameObject _downHint;
        [SerializeField] private RectTransform _currentRectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private RectTransform _itemRectTransform;
        [SerializeField] private LocalizeStringEvent _localizeDescription;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [field: SerializeField] public UsableTypeSO Type { get; private set; }
        public Image MenuItem;
        private List<UsableInfo> _itemList = new();
        private List<MultiInputButton> _buttonList = new();
        private UIItemInventory _itemInformation;

        private void Awake()
        {
            InitData();
        }

        private void OnEnable()
        {
            _inputMediator.EnableMenuInput();
            _inputMediator.MenuNavigateEvent += SelectItemHandle;
        }

        private void OnDisable()
        {
            _inputMediator.MenuNavigateEvent -= SelectItemHandle;
        }

        private void InitData()
        {
            _recyclableScrollRect.DataSource = this;
            _itemList.Clear();
            foreach (var usable in _inventory.UsableItems)
            {
                if (usable.Item.UsableTypeSO == Type)
                {
                    _itemList.Add(usable);
                }
            }
        }

        private void SelectItemHandle(Vector2 arg0)
        {
            _autoScrollRect.UpdateScrollRectTransform();
            ShowScrollHints();
            if (EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>())
            {
                _itemInformation = EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>();
                _localizeDescription.StringReference = _itemInformation.Description;
            }
        }
        public void ActiveItemSelection(bool isActivated)
        {
            foreach (var button in _buttonList)
            {
                button.GetComponent<MultiInputButton>().enabled = isActivated;
            }
        }

        public void Deselect()
        {
            _content.SetActive(false);
            MenuItem.enabled = false;
        }

        public void Select()
        {
            _content.SetActive(true);
            MenuItem.enabled = true;
            ActiveItemSelection(true);
            ShowScrollHints();
            if (_buttonList.Count > 0)
            {
                _buttonList[0].Select();
                SelectItemHandle(Vector2.zero);
            }
        }
        private void ShowScrollHints()
        {
            _upHint.SetActive(ShouldMoveUp);
            _downHint.SetActive(ShouldMoveDown);
        }

        private bool ShouldMoveUp => _currentRectTransform.anchoredPosition.y > _itemRectTransform.rect.height;

        private bool ShouldMoveDown =>
                _currentRectTransform.rect.height - _currentRectTransform.anchoredPosition.y
                > _parentRectTransform.rect.height + _itemRectTransform.rect.height / 2;

        #region PLUGINS

        public int GetItemCount()
        {
            return _itemList.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var item = cell as UIItemInventory;
            item.Init(_itemList[index]);
            _buttonList.Add(item.GetComponent<MultiInputButton>());
        }

        #endregion
    }
}