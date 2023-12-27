using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIInventoryItemList<TItem> : MonoBehaviour where TItem : UIShopItem
    {
        public event Action<TItem> ItemSelected;
        [SerializeField] private UIShopItemPool<TItem> _pool;
        [field: SerializeField] protected PriceMappingDatabase PriceMappingDatabase { get; private set; }
        [field: SerializeField] protected ScrollRect ScrollRect { get; private set; }


        private void OnEnable()
        {
            Clear();
            Render();
        }

        private void OnDisable() => Clear();

        private void Clear()
        {
            foreach (var child in ScrollRect.content.GetComponentsInChildren<TItem>()) Release(child);
        }

        private void Release(TItem item)
        {
            OnUnregisterEvent(item);
            _pool.Release(item);
        }

        protected virtual TItem GetItem(float price)
        {
            var uiShopItem = _pool.Get(price);
            uiShopItem.transform.parent = ScrollRect.content;
            OnRegisterEvent(uiShopItem);
            return uiShopItem;
        }

        protected void OnSelected(TItem item) => ItemSelected?.Invoke(item);
        protected abstract void OnRegisterEvent(TItem uiShopItem);
        protected abstract void OnUnregisterEvent(TItem uiShopItem);

        protected abstract void Render();
    }
}