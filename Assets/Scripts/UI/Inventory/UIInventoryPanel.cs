using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Inventory;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;

namespace CryptoQuest.UI.Inventory
{
    // TODO: rename class
    public class UIInventoryPanel : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [SerializeField] private EItemType _type;
        [SerializeField] private GameObject _upHint;
        [SerializeField] private GameObject _downHint;
        public EItemType Type => _type;
        private Dictionary<EItemType, List<UIItemInventory>> _itemsByType = new();
        public List<ItemInfomation> ItemInfo = new();
        private List<ItemInfomation> _itemList = new List<ItemInfomation>();
        public List<ItemInfomation> ItemList => _itemList;

        private bool _initializedData = false;

        private void Awake()
        {
            InitData();
        }

        private void InitData()
        {
            _recyclableScrollRect.Initialize(this);
            _itemList.Clear();
            foreach (var item in _inventory._items)
            {
                var itemSO = item.ItemSO;
                var type = itemSO.Type;
                if (type != _type) continue;
                _itemList.Add(new ItemInfomation(itemSO, item.Quantity));
            }
        }


        public void Deselect()
        {
            Debug.Log($"Deselect {_type}");
            _content.SetActive(false);
        }

        public void Select()
        {
            Debug.Log($"Select {_type}");
            _content.SetActive(true);
            CheckScrollRect();
        }

        private void CheckScrollRect()
        {
            // _currentRectTransform = _cachedInventories[_currentActivePanel.Type].transform as RectTransform;
            // RectTransform items = _recyclableScrollRect.PrototypeCell;
            // bool shouldMoveUp = _currentRectTransform.anchoredPosition.y > items.rect.height;
            // _upArrowHint.SetActive(shouldMoveUp);
            // bool shouldMoveDown =
            //     _currentRectTransform.rect.height - _currentRectTransform.anchoredPosition.y
            //     > _parentTransform.rect.height + items.rect.height / 2;
            // _downArrowHint.SetActive(shouldMoveDown);
        }

        #region PLUGINS
        public int GetItemCount()
        {
            return _itemList.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var item = cell as UIItemInventory;
            item.Init(_itemList[index]);
        }
        public void SetButtonListener()
        {
            Debug.Log("SetButtonListener");
        }
        #endregion

        private void HandleItemPressed()
        {

        }
    }
}
