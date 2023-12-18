using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIInventoryItemList<TItem> : MonoBehaviour where TItem : UIShopItem
    {
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
            foreach (var child in ScrollRect.content.GetComponentsInChildren<TItem>()) _pool.Release(child);
        }

        protected TItem GetItem(float price)
        {
            var uiShopItem = _pool.Get(price);
            uiShopItem.transform.parent = ScrollRect.content;
            return uiShopItem;
        }

        protected abstract void Render();
    }
}