using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolvableEquipmentList : MonoBehaviour
    {
        public event Action<UIEquipmentItem> EquipmentSelected;
        public event Action<UIEquipmentItem> EquipmentHighlighted;

        [SerializeField] private ScrollRect _scrollRect;
        public Transform Content => _scrollRect.content;
        [SerializeField] private UIEquipmentItem _itemPrefab;

        private IObjectPool<UIEquipmentItem> _itemPool;
        private List<UIEquipmentItem> _cachedItems = new();

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIEquipmentItem>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public void RenderEquipmentsWithException(List<IEquipment> items, UIEquipmentItem exceptionUI = null)
        {
            foreach (var equipment in items)
            {
                if (exceptionUI != null && equipment.Id == exceptionUI.Equipment.Id) continue;
                var equipmentUI = _itemPool.Get();
                equipmentUI.Init(equipment);
            }

            Invoke(nameof(SelectFirstButton), ERROR_PRONE_DELAY);
        }

        // Prevent select right after change state
        private const float ERROR_PRONE_DELAY = 0.05f;
        private void SelectFirstButton()
        {
            foreach (var item in _cachedItems)
            {
                if (item.ButtonUI.interactable)
                {
                    item.ButtonUI.Select();
                    return;
                }
            }
        }

        public void ClearEquipmentsWithException(UIEquipmentItem exceptionUI = null)
        {
            foreach (var item in _cachedItems.ToList())
            {
                if (exceptionUI != null && item.Equipment.Id == exceptionUI.Equipment.Id) continue;
                _itemPool.Release(item);
            }
        }

        private void OnSelectItem(UIEquipmentItem ui)
        {
            EquipmentSelected?.Invoke(ui);
        }

        private void OnHighlightItem(UIEquipmentItem item)
        {
            EquipmentHighlighted?.Invoke(item);
        }

        #region Pool-handler

        private UIEquipmentItem OnCreate()
        {
            var button = Instantiate(_itemPrefab, _scrollRect.content);
            return button;
        }

        private void OnGet(UIEquipmentItem item)
        {
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            item.Selected += OnSelectItem;
            item.Highlighted += OnHighlightItem;
            _cachedItems.Add(item);
        }

        private void OnReturnToPool(UIEquipmentItem item)
        {
            item.ResetItemStates();
            item.gameObject.SetActive(false);
            item.Selected -= OnSelectItem;
            item.Highlighted -= OnHighlightItem;
            _cachedItems.Remove(item);
        }

        private void OnDestroyPool(UIEquipmentItem item)
        {
            item.Selected -= OnSelectItem; // Just in case
            Destroy(item.gameObject);
        }

        #endregion
    }
}