using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.ShopSystem
{
    public class UIShopItemPool<TItem> : MonoBehaviour where TItem : UIShopItem
    {
        [SerializeField] private TItem _prefab;

        private IObjectPool<TItem> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<TItem>(OnCreate, OnGet, OnRelease, OnDestroyEquipmentUI);
        }

        private TItem OnCreate()
        {
            var uiItem = Instantiate(_prefab, transform);
            uiItem.gameObject.SetActive(false);
            return uiItem;
        }

        private void OnGet(TItem uiItem) => uiItem.gameObject.SetActive(true);

        private void OnRelease(TItem item)
        {
            item.transform.SetParent(transform);
            item.gameObject.SetActive(false);
        }

        private void OnDestroyEquipmentUI(TItem item) => Destroy(item.gameObject);

        public TItem Get(float price)
        {
            var uiItem = _pool.Get();
            uiItem.SetPrice(price);
            return uiItem;
        }

        public void Release(TItem item)
        {
            if (item == null || !item.gameObject.activeInHierarchy)
                return;

            _pool.Release(item);
        }
    }
}