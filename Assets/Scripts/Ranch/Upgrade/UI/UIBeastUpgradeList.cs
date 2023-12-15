using System;
using System.Collections.Generic;
using CryptoQuest.UI.Utilities;
using UI.Common;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIBeastUpgradeList : MonoBehaviour
    {
        public event Action<UIBeastUpgradeListDetail> OnInspectingEvent;

        public event Action<bool> IsOpenDetailsEvent;

        [SerializeField] private UIBeastUpgradeListDetail _beast;
        [SerializeField] private ScrollRect _scrollRect;

        private RectTransform _scrollViewContent => _scrollRect.content;
        private IObjectPool<UIBeastUpgradeListDetail> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<UIBeastUpgradeListDetail>(OnCreate, OnGet, OnRelease, OnDestroyBeastUI);
        }

        public void FillBeasts(List<Beast.Beast> beasts)
        {
            CleanUpScrollView();
            InstantiateBeast(beasts);
        }

        private void InstantiateBeast(List<Beast.Beast> beasts)
        {
            Clear();
            foreach (var beast in beasts)
            {
                var uiBeast = GetItem();
                uiBeast.Init(beast);
                uiBeast.OnInspectingBeast += OnInspectingItem;
                uiBeast.OnSubmit += OnItemSelected;
            }

            if (_hasFocus)
            {
                _hasFocus = false;
                return;
            }

            _hasFocus = TryFocus();

            IsOpenDetailsEvent?.Invoke(_scrollRect.content.childCount > 0);
        }

        private void OnItemSelected(UIBeastUpgradeListDetail obj) { }

        private void OnInspectingItem(UIBeastUpgradeListDetail listDetail)
        {
            OnInspectingEvent?.Invoke(listDetail);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }

        #region Pool

        private void Clear()
        {
            foreach (var child in _scrollRect.GetComponentsInChildren<UIBeastUpgradeListDetail>()) Release(child);
        }

        private UIBeastUpgradeListDetail OnCreate()
        {
            var uiBeast = Instantiate(_beast, _scrollViewContent);
            uiBeast.gameObject.SetActive(false);
            return uiBeast;
        }

        private void OnGet(UIBeastUpgradeListDetail uiItem) => uiItem.gameObject.SetActive(true);

        private void OnRelease(UIBeastUpgradeListDetail item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnDestroyBeastUI(UIBeastUpgradeListDetail item) => Destroy(item.gameObject);

        private UIBeastUpgradeListDetail GetItem()
        {
            var uiBeast = _pool.Get();
            return uiBeast;
        }

        private void Release(UIBeastUpgradeListDetail item)
        {
            _pool.Release(item);
        }

        private bool _interactable;
        private bool _hasFocus;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                foreach (var selectable in _scrollViewContent.GetComponentsInChildren<Selectable>())
                    selectable.interactable = value;
                if (_interactable) Focus();
            }
        }

        private bool TryFocus()
        {
            Interactable = false;
            var firstChild = GetComponentInChildren<UIBeastUpgradeListDetail>();
            if (!firstChild) return false;
            Interactable = true;
            return true;
        }

        private void Focus()
        {
            _scrollViewContent.GetOrAddComponent<SelectFirstChildInList>().Select();
        }

        #endregion
    }
}