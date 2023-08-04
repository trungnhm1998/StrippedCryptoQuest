using System.Collections.Generic;
using CryptoQuest.Data.Item;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
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
        [SerializeField] private GameObject _content;
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [SerializeField] private UsableTypeSO _type;
        [SerializeField] private GameObject _upHint;
        [SerializeField] private GameObject _downHint;
        [SerializeField] private RectTransform _currentRectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private RectTransform _itemRectTransform;
        [SerializeField] private LocalizeStringEvent _localizeDescription;
        [SerializeField] private Image _tabImage;
        public UsableTypeSO Type => _type;
        private List<UsableInformation> _itemList = new();
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
            foreach (var item in _inventory.UsableItem)
            {
                var itemSO = item.ItemSO;
                var type = itemSO.UsableTypeSO;
                if (type != _type) continue;

                _itemList.Add(new UsableInformation(itemSO, item.Quantity));
            }
        }

        private void SelectItemHandle(Vector2 arg0)
        {
            _autoScrollRect.UpdateScrollRectTransform();
            CheckScrollRect();
            if (EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>())
            {
                _itemInformation = EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>();
                _localizeDescription.StringReference = _itemInformation.Description;
            }
        }

        public void Deselect()
        {
            _content.SetActive(false);
            _tabImage.enabled = false;
        }

        public void Select()
        {
            _content.SetActive(true);
            _tabImage.enabled = true;
            CheckScrollRect();
            if (_buttonList.Count > 0)
            {
                _buttonList[0].Select();
                SelectItemHandle(Vector2.zero);
            }
        }

        private void CheckScrollRect()
        {
            bool shouldMoveUp = _currentRectTransform.anchoredPosition.y > _itemRectTransform.rect.height;
            _upHint.SetActive(shouldMoveUp);
            bool shouldMoveDown =
                _currentRectTransform.rect.height - _currentRectTransform.anchoredPosition.y
                > _parentRectTransform.rect.height + _itemRectTransform.rect.height / 2;
            _downHint.SetActive(shouldMoveDown);
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
            _buttonList.Add(item.GetComponent<MultiInputButton>());
        }

        #endregion
    }
}