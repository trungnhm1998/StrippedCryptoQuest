using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolvableEquipmentList : MonoBehaviour
    {
        public event Action<UIEquipmentItem> EquipmentSelected;

        [SerializeField] private ScrollRect _scrollRect;
        public Transform Content => _scrollRect.content;
        [SerializeField] private UIEquipmentItem _itemPrefab;

        private IObjectPool<UIEquipmentItem> _itemPool;

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIEquipmentItem>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public void RenderEquipments(List<IEquipment> items)
        {
            foreach (var equipment in items)
            {
                var equipmentUI = _itemPool.Get();
                equipmentUI.Init(equipment);
            }
        }

        public void ClearEquipmentsWithException(UIEquipmentItem exceptionUI = null)
        {
            foreach (var item in _scrollRect.content.GetComponentsInChildren<UIEquipmentItem>())
            {
                if (exceptionUI != null && item == exceptionUI) continue;
                _itemPool.Release(item);
            }
        }

        private void OnSelectItem(UIEquipmentItem ui)
        {
            EquipmentSelected?.Invoke(ui);
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
        }

        private void OnReturnToPool(UIEquipmentItem item)
        {
            item.gameObject.SetActive(false);
            item.Selected -= OnSelectItem;
        }

        private void OnDestroyPool(UIEquipmentItem item)
        {
            item.Selected -= OnSelectItem; // Just in case
            Destroy(item.gameObject);
        }

        #endregion
    }
}