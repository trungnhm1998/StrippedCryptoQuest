using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIInventoryItemList<TItem> : MonoBehaviour where TItem : UIShopItem
    {
        [field: SerializeField] protected PriceMappingDatabase PriceMappingDatabase { get; private set; }
        [SerializeField] private InventorySO _inventory;
        protected InventorySO Inventory => _inventory;
        [SerializeField] private TItem _prefab;
        protected TItem Prefab => _prefab;
        [field: SerializeField] protected ScrollRect ScrollRect { get; private set; }

        private IObjectPool<TItem> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<TItem>(OnCreate, OnGet, OnRelease, OnDestroyEquipmentUI);
        }

        private void OnEnable()
        {
            Clear();
            Render();
        }

        private void Clear()
        {
            foreach (var child in ScrollRect.content.GetComponentsInChildren<TItem>()) Release(child);
        }

        protected abstract void Render();

        protected virtual TItem OnCreate()
        {
            var uiItem = Instantiate(Prefab, ScrollRect.content);
            uiItem.gameObject.SetActive(false);
            return uiItem;
        }

        protected virtual void OnGet(TItem uiItem) => uiItem.gameObject.SetActive(true);

        protected virtual void OnRelease(TItem item) => item.gameObject.SetActive(false);

        protected virtual void OnDestroyEquipmentUI(TItem item) => Destroy(item.gameObject);

        public virtual TItem GetItem(float price)
        {
            var uiItem = _pool.Get();
            uiItem.SetPrice(price);
            return uiItem;
        }

        protected virtual void Release(TItem item) => _pool.Release(item);
    }
}