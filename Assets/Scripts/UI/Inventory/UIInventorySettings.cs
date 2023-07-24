using System;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Item.Inventory;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventorySettings : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Serializable]
        public class ItemsMenu
        {
            public List<ItemSO> Items;
            public bool InitializedData = false;
        }

        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private UIInventoryPanel _inventoryPanel;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UIInventoryTabButton> _tabInventoryButton;
        [SerializeField] private List<ItemsMenu> _itemMenu;
        [SerializeField] private List<GameObject> _inventory;
        [SerializeField] private RectTransform _parentTransform;
        [SerializeField] private GameObject _upArrowHint;
        [SerializeField] private GameObject _downArrowHint;
        private RectTransform _currentRectTransform;
        private UIInventoryTabButton _currentMenu;
        private Button _currentItem;
        private int _dataNumber = 0;
        private List<ItemInformation> _itemList = new List<ItemInformation>();

        private void Awake()
        {
            InitData();
            _recyclableScrollRect.DataSource = this;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked += SelectTab;
            }
        }

        private void OnEnable()
        {
            _inputMediator.EnableMenuInput();
            _inputMediator.NextSelectionMenu += SelectionNextMenu;
            _inputMediator.PreviousSelectionMenu += SelectionPreviousMenu;
            _inputMediator.MenuNavigateEvent += SelectItemHandle;
            _selectionHandler.UpdateDefault(_tabInventoryButton[0].gameObject);
        }

        private void OnDisable()
        {
            _inputMediator.NextSelectionMenu -= SelectionNextMenu;
            _inputMediator.PreviousSelectionMenu -= SelectionPreviousMenu;
            _inputMediator.MenuNavigateEvent -= SelectItemHandle;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked -= SelectTab;
            }
        }

        private void SelectItemHandle()
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            _currentItem = currentSelected.GetComponent<Button>();
            _currentItem.Select();
            _autoScrollRect.UpdateScrollRectTransform();
            CheckScrollRect();
        }

        private void SelectionNextMenu()
        {
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                if (_currentMenu != _tabInventoryButton[i])
                    continue;
                int nextIndex = (i + 1) % _tabInventoryButton.Count;
                _currentMenu = _tabInventoryButton[nextIndex];
                SelectTab(nextIndex);
                break;
            }
        }

        private void SelectionPreviousMenu()
        {
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                int nextIndex;
                if (_currentMenu != _tabInventoryButton[i])
                    continue;
                if (i == 0)
                {
                    nextIndex = _tabInventoryButton.Count - 1;
                }
                else
                {
                    nextIndex = (i - 1) % _tabInventoryButton.Count;
                    _currentMenu = _tabInventoryButton[nextIndex];
                }
                SelectTab(nextIndex);
                break;
            }
        }

        private void InitData()
        {
            _currentMenu = _tabInventoryButton[_dataNumber];
            _itemList.Clear();
            foreach (ItemSO item in _itemMenu[_dataNumber].Items)
            {
                _itemList.Add(new ItemInformation(item));
                // _inventoryPanel.ItemInfo.Add(new ItemInformation(item));
            }
            _inventoryPanel.ItemInfo.AddRange(_itemList);
            _itemMenu[_dataNumber].InitializedData = true;
            _recyclableScrollRect.Initialize(this);
            CheckScrollRect();
        }

        public void CheckScrollRect()
        {
            if (_currentRectTransform == null)
                _currentRectTransform = _inventory[_dataNumber].transform as RectTransform;
            RectTransform items = _recyclableScrollRect.PrototypeCell;
            bool shouldMoveUp = _currentRectTransform.anchoredPosition.y > items.rect.height;
            _upArrowHint.SetActive(shouldMoveUp);
            bool shouldMoveDown =
                _currentRectTransform.rect.height - _currentRectTransform.anchoredPosition.y
                > _parentTransform.rect.height + items.rect.height / 2;
            _downArrowHint.SetActive(shouldMoveDown);
        }

        private void SelectItem()
        {
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                if (_currentMenu == _tabInventoryButton[i] && _itemMenu[i].Items.Count != 0)
                {
                    _currentItem = _inventoryPanel.Item[i].ListButton[0];
                    _currentItem.Select();
                }
            }
        }

        private void SelectTab(int dataNumber)
        {
            for (int i = 0; i < _itemMenu.Count; i++)
            {
                bool isMatched = (i == dataNumber);
                _tabInventoryButton[i].GetComponent<Image>().enabled = isMatched;
                _inventory[i].SetActive(isMatched);
                if (isMatched)
                    ShowTab(i);
            }
        }

        private void ShowTab(int dataNumber)
        {
            _dataNumber = dataNumber;
            _currentMenu = _tabInventoryButton[dataNumber];
            _currentMenu.Select();
            _recyclableScrollRect.content = _inventory[dataNumber].transform as RectTransform;
            _autoScrollRect.ContentRectTransform =
                _inventory[dataNumber].transform as RectTransform;
            if (!_itemMenu[dataNumber].InitializedData)
                InitData();
            if (_inventoryPanel.Item[dataNumber].ListButton.Count != 0)
                SelectItem();
        }

        public int GetItemCount()
        {
            return _itemList.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var item = cell as UIItemStatus;
            item.ConfigureCell(_itemList[index], index);
            for (int i = 0; i < _itemMenu.Count; i++)
            {
                if (_currentMenu == _tabInventoryButton[i])
                {
                    _inventoryPanel.AddItem(item.GetComponent<Button>(), i);
                }
            }
        }

        public void SetButtonListener()
        {
            _inventoryPanel.UpdateButton();
            SelectItem();
        }
    }
}
