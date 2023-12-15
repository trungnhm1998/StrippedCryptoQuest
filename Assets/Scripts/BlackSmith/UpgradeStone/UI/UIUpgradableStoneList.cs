using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIUpgradableStoneList : MonoBehaviour
    {
        public event Action<UIUpgradableStone> StoneSelected;
        public event Action<IMagicStone> StoneInspected;
        public event Action StoneDeselected;


        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIUpgradableStone _stonePrefab;
        public Transform Content => _scrollRect.content;
        protected IObjectPool<UIUpgradableStone> _itemPool;
        protected const float ERROR_PRONE_DELAY = 0.05f;
        private List<UIUpgradableStone> _cachedItems = new();

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIUpgradableStone>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public virtual void RenderStones(List<IMagicStone> items)
        {
            foreach (var stone in items)
            {
                var equipmentUI = _itemPool.Get();
                equipmentUI.Initialize(stone);
                equipmentUI.MaterialTag.SetActive(false);
            }

            Invoke(nameof(SelectFirstButton), ERROR_PRONE_DELAY);
        }

        protected void SelectFirstButton()
        {
            foreach (var item in _cachedItems)
            {
                if (item.Button.interactable)
                {
                    item.Button.Select();
                    StoneInspected?.Invoke(item.MagicStone);
                    return;
                }
            }
        }

        public void ClearStonesWithException(UIUpgradableStone exceptionUI = null)
        {
            foreach (var item in _cachedItems.ToList())
            {
                if (exceptionUI != null && item.MagicStone.Definition == exceptionUI.MagicStone.Definition &&
                    item.MagicStone.Level == exceptionUI.MagicStone.Level) continue;
                _itemPool.Release(item);
            }
        }

        private void OnInspectItem(UIUpgradableStone ui)
        {
            StoneInspected?.Invoke(ui.MagicStone);
        }

        private void OnDeselectItem()
        {
            StoneDeselected?.Invoke();
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
            item.Inspected += OnInspectItem;
            item.DeSelected += OnDeselectItem;
            item.Selected += OnSelectItem;
            _cachedItems.Add(item);
        }

        protected virtual void OnSelectItem(UIUpgradableStone item)
        {
            StoneSelected?.Invoke(item);
        }

        private void OnReturnToPool(UIUpgradableStone item)
        {
            item.gameObject.SetActive(false);
            item.Inspected -= OnInspectItem;
            item.DeSelected -= OnDeselectItem;
            item.Selected -= OnSelectItem;
            _cachedItems.Remove(item);
        }

        private void OnDestroyPool(UIUpgradableStone item)
        {
            item.Inspected -= OnInspectItem;
            item.DeSelected -= OnDeselectItem;
            item.Selected -= OnSelectItem;
            Destroy(item.gameObject);
        }

        #endregion
    }
}