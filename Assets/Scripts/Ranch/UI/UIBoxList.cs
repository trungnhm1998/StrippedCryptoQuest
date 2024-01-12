using CryptoQuest.UI.Common;
using CryptoQuest.UI.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIBoxList<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private ScrollRect _scrollRect;
        protected ScrollRect ScrollView => _scrollRect;
        protected RectTransform ScrollViewContent => _scrollRect.content;

        [SerializeField] private T _instantiatePrefab;

        private IObjectPool<T> _pool;

        private bool _interactable;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                foreach (var selectable in ScrollViewContent.GetComponentsInChildren<Selectable>())
                    selectable.interactable = value;
                if (_interactable) Focus();
            }
        }

        private void Start()
        {
            _pool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroyUI);
        }

        public virtual void Clear()
        {
            foreach (var child in _scrollRect.GetComponentsInChildren<T>()) Release(child);
        }

        protected virtual T OnCreate()
        {
            var uiItem = Instantiate(_instantiatePrefab, ScrollViewContent);
            uiItem.gameObject.SetActive(false);
            return uiItem;
        }

        protected virtual void OnGet(T uiItem) => uiItem.gameObject.SetActive(true);

        protected virtual void OnRelease(T item) => item.gameObject.SetActive(false);

        protected virtual void OnDestroyUI(T item) => Destroy(item.gameObject);

        protected virtual T GetItem()
        {
            var uiItem = _pool.Get();
            return uiItem;
        }

        protected virtual void Release(T item) => _pool.Release(item);

        public bool TryFocus()
        {
            Interactable = false;
            var firstChild = GetComponentInChildren<T>();
            if (!firstChild) return false;
            Interactable = true;
            EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
            return true;
        }

        private void Focus()
        {
            var lastSelected = ScrollViewContent.GetOrAddComponent<CacheButtonSelector>().LastSelected;
            if (lastSelected == null)
                ScrollViewContent.GetOrAddComponent<SelectFirstChildInList>().Select();
            else
                EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
}