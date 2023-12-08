using System;
using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStoneUI.UI
{
    public class UIUpgradableStoneList : MonoBehaviour
    {
        public event Action<UIUpgradableStone> EquipmentSelected;

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIUpgradableStone _stonePrefab;

        private IObjectPool<UIUpgradableStone> _itemPool;

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIUpgradableStone>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public void RenderEquipments(List<IMagicStone> items)
        {
            foreach (var stone in items)
            {
                var equipmentUI = _itemPool.Get();
                equipmentUI.Initialize(stone);
            }
        }

        public void ClearEquipmentsWithException(UIUpgradableStone exceptionUI = null)
        {
            foreach (var item in _scrollRect.content.GetComponentsInChildren<UIUpgradableStone>())
            {
                if (exceptionUI != null && item == exceptionUI) continue;
                _itemPool.Release(item);
            }
        }

        private void OnSelectItem(UIUpgradableStone ui)
        {
            EquipmentSelected?.Invoke(ui);
        }

        #region Pool-handler

        private UIUpgradableStone OnCreate()
        {
            var button = Instantiate(_stonePrefab, _scrollRect.content);
            return button;
        }

        private void OnGet(UIUpgradableStone item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            item.Pressed += OnSelectItem;
        }

        private void OnReturnToPool(UIUpgradableStone item)
        {
            item.gameObject.SetActive(false);
            item.Pressed -= OnSelectItem;
        }

        private void OnDestroyPool(UIUpgradableStone item)
        {
            item.Pressed -= OnSelectItem;
            Destroy(item.gameObject);
        }

        #endregion
    }
}