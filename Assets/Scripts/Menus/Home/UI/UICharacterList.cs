using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Menus.Home.UI
{
    public class UICharacterList : MonoBehaviour
    {
        [SerializeField] private Transform _scrollRectContent;
        [SerializeField] private UIListItem _itemPrefab;

        private IObjectPool<UIListItem> _pool;
        private List<UIListItem> _items = new();

        private void Awake()
        {
            _pool ??= new ObjectPool<UIListItem>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        public void SetData(List<Obj.Character> data)
        {
            ReleaseAllItemInPool();
            foreach (var heroData in data)
            {
                UIListItem item = _pool.Get();
                item.SetInfo(heroData);
            }
        }

        private UIListItem OnCreate()
        {
            var item = Instantiate(_itemPrefab, _scrollRectContent);
            return item;
        }

        private void OnGet(UIListItem item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            _items.Add(item);
        }

        private void OnRelease(UIListItem item) => item.gameObject.SetActive(false);

        private void OnDestroyPool(UIListItem item) => Destroy(item.gameObject);

        private void ReleaseAllItemInPool()
        {
            foreach (var item in _items)
                _pool.Release(item);

            _items = new();
        }
    }
}