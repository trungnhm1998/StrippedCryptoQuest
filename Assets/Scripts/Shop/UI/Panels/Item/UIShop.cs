using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Shop;
using CryptoQuest.System;
using CryptoQuest.Shop.UI.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Shop.UI.Item;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public abstract class UIShop : MonoBehaviour
    {
        [Header("Raise Event")]
        public Action<IShopItem> OnSubmit;

        [Header("Shop Config")]
        [SerializeField] protected GameObject _content;
        [SerializeField] protected UIShopItem _itemTemplate;
        [SerializeField] private Transform _scrollContentParent;
        [SerializeField] private PreviewItem _previewItem;

        [field: SerializeField] public ShopStateSO ShopState { get; private set; }

        public InventorySO _inventorySO { get; private set; }

        protected readonly List<UIShopItem> _uiItems = new();

        protected UIShopItem _defaultItem;

        private void Awake()
        {
            _inventorySO = ServiceProvider.GetService<IInventoryController>().Inventory;
        }

        protected abstract void InitItem();


        public void Hide()
        {
            _content.SetActive(false);
            _previewItem.Hide();
        }

        public virtual void Show()
        {
            ResetShopItem();
            InitItem();
            _content.SetActive(true);
            StartCoroutine(SelectDefaultButton());
        }

        protected IEnumerator SelectDefaultButton()
        {
            yield return null;
            if(_scrollContentParent.childCount > 0)
            {
                _defaultItem = _scrollContentParent.GetComponentInChildren<UIShopItem>();
                _defaultItem?.Select();
            }    
        }

        private void OnItemSubmit(IShopItem shopItemInfo)
        {
            OnSubmit?.Invoke(shopItemInfo);
        }

        private void OnItemSelected(IShopItem shopItemInfo)
        {
            shopItemInfo.PreviewItem(_previewItem);
        }

        protected void InstantiateItem(IShopItem itemShopData, bool isBuy)
        {
            var uiItem = GetShopItem();

            uiItem.Init(itemShopData, isBuy);
            uiItem.gameObject.SetActive(true);
            uiItem.gameObject.transform.SetAsLastSibling();
        }

        private UIShopItem GetShopItem()
        {
            foreach (var uiItem in _uiItems)
            {
                if (!uiItem.gameObject.activeSelf)
                {
                    return uiItem;
                }
            }

            var item = Instantiate(_itemTemplate, _scrollContentParent);
            item.gameObject.SetActive(false);
            item.OnSubmit += OnItemSubmit;
            item.OnSelected += OnItemSelected;
            _uiItems.Add(item);

            return item;
        }

        protected void ResetShopItem()
        {
            foreach (var uiItem in _uiItems)
            {
                uiItem.gameObject.SetActive(false);
            }
        }
    }
}
