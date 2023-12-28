using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Inventory;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class UICharacterList : MonoBehaviour
    {
        public event Action<HeroSpec> InspectingHero;
        [SerializeField] private HeroInventorySO _inventory;
        [SerializeField] private Transform _scrollRectContent;
        [SerializeField] private UIListItem _itemPrefab;
        [SerializeField] private SelectFirstChildInList _selectFirstChildComp;

        private IObjectPool<UIListItem> _pool;
        private List<UIListItem> _items = new();

        private void Awake()
        {
            _pool = new ObjectPool<UIListItem>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        private void OnEnable()
        {
            ReleaseAllItemInPool();
            foreach (var heroData in _inventory.OwnedHeroes)
            {
                UIListItem item = _pool.Get();
                item.SetInfo(heroData);
            }

            _selectFirstChildComp.Select();
        }

        private void OnInspectItem(HeroSpec spec) => InspectingHero?.Invoke(spec);

        #region Pool

        private UIListItem OnCreate()
        {
            var item = Instantiate(_itemPrefab, _scrollRectContent);
            return item;
        }

        private void OnGet(UIListItem item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            item.InspectCharacterEvent += OnInspectItem;
            _items.Add(item);
        }

        private void OnRelease(UIListItem item)
        {
            item.gameObject.SetActive(false);
            item.InspectCharacterEvent -= OnInspectItem;
        }

        private void OnDestroyPool(UIListItem item) => Destroy(item.gameObject);

        private void ReleaseAllItemInPool()
        {
            foreach (var item in _items)
                _pool.Release(item);

            _items = new();
        }

        #endregion
    }
}