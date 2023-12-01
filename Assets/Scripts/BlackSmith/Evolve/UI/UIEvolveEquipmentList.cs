using System;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIEquipmentItem _itemPrefab;

        public event Action OnEquipmentRendered;
        public event Action<UIEquipmentItem> OnItemSelected;
        public event Action<UIEquipmentItem> OnItemSubmitted;

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

        public void RenderEquipments(List<IEvolvableEquipment> data)
        {
            ReturnAllItemsToPool();

            for (int i = 0; i < data.Count; i++)
            {
                UIEquipmentItem uIEquipmentItem = InstantiateNewEquipmentUI(data[i]);
                uIEquipmentItem.InspectingEquipmentEvent += HandleInspectingEquipment;
                uIEquipmentItem.SelectedEquipmentEvent += HandleSelectedEquipment;
            }

            OnEquipmentRendered?.Invoke();
        }

        private void HandleInspectingEquipment(UIEquipmentItem equipmentUi)
        {
            OnItemSelected?.Invoke(equipmentUi);
        }

        private void HandleSelectedEquipment(UIEquipmentItem equipmentUi)
        {
            OnItemSubmitted?.Invoke(equipmentUi);
        }

        private UIEquipmentItem InstantiateNewEquipmentUI(IEvolvableEquipment equipmentData)
        {
            UIEquipmentItem equipmentUi = _itemPool.Get();
            equipmentUi.SetItemData(equipmentData);

            _equipmentList.Add(equipmentUi);
            return equipmentUi;
        }

        public void SelectDefaultButton()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        #region Pool-handler

        public void ReturnAllItemsToPool()
        {
            foreach (var item in _items)
            {
                item.InspectingEquipmentEvent -= HandleInspectingEquipment;
                item.SelectedEquipmentEvent -= HandleSelectedEquipment;
                _itemPool.Release(item);
            }
            _items.Clear();
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