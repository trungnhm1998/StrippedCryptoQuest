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
        [SerializeField] private RectTransform _currentRectTransform;
        [SerializeField] private Button _currentItem;
        private int _indexMenuNumber = 0;
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
            _indexMenuNumber++;
            _indexMenuNumber = _indexMenuNumber >= _tabInventoryButton.Count ? 0 : _indexMenuNumber;
            SelectTab(_indexMenuNumber);
        }

        private void SelectionPreviousMenu()
        {
            _indexMenuNumber--;
            _indexMenuNumber = _indexMenuNumber < 0 ? _tabInventoryButton.Count - 1 : _indexMenuNumber;
            SelectTab(_indexMenuNumber);
        }

        private void InitData()
        {
            _itemList.Clear();
            foreach (ItemSO item in _itemMenu[_indexMenuNumber].Items)
            {
                _itemList.Add(new ItemInformation(item));
            }
            _inventoryPanel.ItemInfo.AddRange(_itemList);
            _itemMenu[_indexMenuNumber].InitializedData = true;
            _recyclableScrollRect.Initialize(this);
        }

        private void CheckScrollRect()
        {
            _currentRectTransform = _inventory[_indexMenuNumber].transform as RectTransform;
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
            if (_itemMenu[_indexMenuNumber].Items.Count != 0)
            {
                _currentItem = _inventoryPanel.ListItem[_indexMenuNumber].ListButton[0];
                _currentItem.Select();
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
            CheckScrollRect();
        }

        private void ShowTab(int index)
        {
            _recyclableScrollRect.content = _inventory[index].transform as RectTransform;
            _autoScrollRect.ContentRectTransform =
                _inventory[index].transform as RectTransform;
            if (!_itemMenu[index].InitializedData)
                InitData();
            if (_inventoryPanel.ListItem[index].ListButton.Count != 0)
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
            _inventoryPanel.AddItem(item.GetComponent<Button>(), _indexMenuNumber);
        }

        public void SetButtonListener()
        {
            _inventoryPanel.UpdateButton();
            SelectItem();
            CheckScrollRect();
        }
    }
}
