using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIStoneList : MonoBehaviour
    {
        [SerializeField] private Transform _scrollRectContent;
        [SerializeField] private UISingleStone _singleStonePrefab;

        private IObjectPool<UISingleStone> _pool;
        private List<UISingleStone> _items = new();

        private void Awake()
        {
            _pool ??= new ObjectPool<UISingleStone>(OnCreate, OnGet, OnRelease, OnDestroyPool);
        }

        public void SetData(List<IMagicStone> stoneDataList)
        {
            ReleaseAllItemInPool();
            foreach (var stoneData in stoneDataList)
            {
                UISingleStone item = _pool.Get();
                item.SetInfo(stoneData);
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
            _items.Add(item);
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