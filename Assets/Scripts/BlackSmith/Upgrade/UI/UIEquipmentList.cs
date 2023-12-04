using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIEquipmentList : MonoBehaviour
    {
        public Action<UIUpgradeEquipment> OnSelectedUpgradeItem;
        public Action<UIUpgradeEquipment> OnSubmitUpgradeItem;

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UIUpgradeEquipment _prefab;

        private IObjectPool<UIUpgradeEquipment> _pool;
        private List<UIUpgradeEquipment> _items = new();

        private UIUpgradeEquipment _selectedUI;

        private void Awake()
        {
            _pool = new ObjectPool<UIUpgradeEquipment>(OnCreateItem, OnGetItem, OnReleaseItem, OnDestroyItem);
        }

        private void OnItemPressed(UIUpgradeEquipment item)
        {
            OnSubmitUpgradeItem?.Invoke(item);
        }

        private void OnItemSelected(UIUpgradeEquipment item)
        {
            OnSelectedUpgradeItem?.Invoke(item);
            _selectedUI = item;
        }

        public void InitUI(IUpgradeModel model)
        {
            CleanUpPool();

            for (int i = 0; i < model.Equipments.Count; i++)
            {
                var item = _pool.Get();
                item.SetupUI(model.Equipments[i]);
                item.OnItemSelected += OnItemSelected;
                item.OnSubmit += OnItemPressed;
            }

            StartCoroutine(SelectFirstButton());
        }

        public void SetInteractable(bool value)
        {
            foreach (var item in _items)
            {
                item.Button.interactable = value;
            } 
        }

        public void ResetSelected()
        {
            if (_selectedUI == null) return;
            _selectedUI.SetConfirmSelected(false);
        }

        private void CleanUpPool()
        {
            while (_items.Count > 0)
            {
                _pool.Release(_items[0]);
            }

            _items.Clear();
        }

        private IEnumerator SelectFirstButton()
        {
            yield return null;
            if (_items.Count == 0) yield break;
            EventSystem.current.SetSelectedGameObject(_items[0].gameObject);
        }

        private UIUpgradeEquipment OnCreateItem() => Instantiate(_prefab, _scrollRect.content);

        private void OnGetItem(UIUpgradeEquipment item)
        {
            _items.Add(item);
            item.transform.SetAsLastSibling();
            item.gameObject.SetActive(true);
        }

        private void OnReleaseItem(UIUpgradeEquipment item)
        {
            _items.Remove(item);
            item.gameObject.SetActive(false);
        }

        private void OnDestroyItem(UIUpgradeEquipment item)
        {
            Destroy(item);
        }
    }
}