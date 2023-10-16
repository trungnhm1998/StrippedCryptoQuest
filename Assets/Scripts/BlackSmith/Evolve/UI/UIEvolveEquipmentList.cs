using System;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _itemPrefab;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _finishedRenderEvent;

        private List<UIEquipmentItem> _equipmentList = new();
        public List<UIEquipmentItem> EquipmentList { get => _equipmentList; }

        private IObjectPool<UIEquipmentItem> _itemPool;
        private MultiInputButton _firstButton;
        private List<UIEquipmentItem> _items = new();

        private void Awake()
        {
            _itemPool ??= new ObjectPool<UIEquipmentItem>(OnCreate, OnGet,
                OnReturnToPool, OnDestroyPool);
        }

        public void RenderEquipments(List<IEvolvableData> data)
        {
            ReturnAllItemsToPool();

            for (int i = 0; i < data.Count; i++)
            {
                InstantiateNewEquipmentUI(data[i]);
            }

            SelectDefaultButton();
            _finishedRenderEvent.Invoke();
        }

        private void InstantiateNewEquipmentUI(IEvolvableData equipmentData)
        {
            UIEquipmentItem equipmentUi = _itemPool.Get();
            equipmentUi.SetItemData(equipmentData);

            _equipmentList.Add(equipmentUi);
        }

        private void SelectDefaultButton()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        #region Pool-handler

        private void ReturnAllItemsToPool()
        {
            foreach (var item in _items)
            {
                _itemPool.Release(item);
            }
            _items = new();
            _firstButton = null;
        }

        private UIEquipmentItem OnCreate()
        {
            var button = Instantiate(_itemPrefab, _scrollRect.content);
            return button;
        }

        private void OnGet(UIEquipmentItem item)
        {
            if (_firstButton == null) _firstButton = item.GetComponentInChildren<MultiInputButton>();
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
            _items.Add(item);
        }

        private void OnReturnToPool(UIEquipmentItem item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnDestroyPool(UIEquipmentItem item)
        {
            Destroy(item.gameObject);
        }

        #endregion
    }
}