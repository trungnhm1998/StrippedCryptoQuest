using System;
using System.Collections.Generic;
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
            ReturnAllItemsToPool();
            foreach (var equipment in items)
            {
                var equipmentUI = _itemPool.Get();
                equipmentUI.Init(equipment);
            }

            EventSystem.current.SetSelectedGameObject(_scrollRect.content.GetChild(0).gameObject);
        }

        private void OnSelectItem(UIEquipmentItem ui)
        {
            EquipmentSelected?.Invoke(ui);
        }

        public void Filter(UIEquipmentItem baseItem)
        {
            foreach (var equipmentUI in _scrollRect.content.GetComponentsInChildren<UIEquipmentItem>())
            {
                if (equipmentUI == baseItem) continue;
                equipmentUI.ResetItemStates();
                if (equipmentUI.Equipment.Data.ID == baseItem.Equipment.Data.ID) continue;
                _itemPool.Release(equipmentUI);
            }
            
            baseItem.transform.SetAsFirstSibling();
        }

        #region Pool-handler

        private void ReturnAllItemsToPool()
        {
            foreach (var item in _scrollRect.content.GetComponentsInChildren<UIEquipmentItem>())
                _itemPool.Release(item);
        }

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