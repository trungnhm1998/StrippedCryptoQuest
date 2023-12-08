using System;
using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIStoneList : MonoBehaviour
    {
        [SerializeField] private MagicStoneInventory _inventory;
        [SerializeField] private Transform _scrollRectContent;
        [SerializeField] private UISingleStone _singleStonePrefab;

        private IObjectPool<UISingleStone> _pool;
        private List<UISingleStone> _items = new();

        private void Awake()
        {
            _pool ??= new ObjectPool<UISingleStone>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        private void OnEnable()
        {
            RenderAll();
        }

        public void Filter(MagicStoneDef stoneDef)
        {
            if (stoneDef == null) return;
            var filteredStone = new List<IMagicStone>();
            foreach (var magicStone in _inventory.MagicStones)
            {
                if (magicStone.Definition != stoneDef) continue;
                filteredStone.Add(magicStone);
            }

            RenderWithData(filteredStone);
        }

        public void RenderAll()
        {
            RenderWithData(_inventory.MagicStones);
        }

        private void RenderWithData(List<IMagicStone> stoneDataList)
        {
            ReleaseAllItemInPool();
            foreach (var stoneData in stoneDataList)
            {
                UISingleStone item = _pool.Get();
                item.SetInfo(stoneData);
                _items.Add(item);
            }
        }

        private UISingleStone OnCreate()
        {
            var item = Instantiate(_singleStonePrefab, _scrollRectContent);
            return item;
        }

        private void OnGet(UISingleStone item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
        }

        private void OnRelease(UISingleStone item) => item.gameObject.SetActive(false);

        private void OnDestroyPool(UISingleStone item) => Destroy(item.gameObject);

        private void ReleaseAllItemInPool()
        {
            foreach (var item in _items)
                _pool.Release(item);

            _items = new();
        }
    }
}