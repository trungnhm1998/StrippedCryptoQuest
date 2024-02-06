using CryptoQuest.UI.Common;
using CryptoQuest.UI.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class UIDimensionalBoxList<TItem> : MonoBehaviour where TItem : Component
    {
        [SerializeField] private ScrollRect _scrollView;
        protected ScrollRect ScrollView => _scrollView;
        protected RectTransform ScrollViewContent => _scrollView.content;

        [FormerlySerializedAs("_equipmentPrefab")] [SerializeField] private TItem _instantiatePrefab;

        private IObjectPool<TItem> _pool;

        protected virtual void Start()
        {
            _pool = new ObjectPool<TItem>(OnCreate, OnGet, OnRelease, OnDestroyEquipmentUI);
        }

        public virtual void Clear()
        {
            foreach (var child in _scrollView.GetComponentsInChildren<TItem>()) Release(child);
        }

        protected virtual TItem OnCreate()
        {
            var uiEquipment = Instantiate(_instantiatePrefab, ScrollViewContent);
            uiEquipment.gameObject.SetActive(false);
            return uiEquipment;
        }

        protected virtual void OnGet(TItem uiItem)
        {
            uiItem.gameObject.SetActive(true);
            uiItem.transform.SetAsLastSibling();
        }

        protected virtual void OnRelease(TItem item)
        {
            item.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyEquipmentUI(TItem item) => Destroy(item.gameObject);

        public virtual TItem GetItem()
        {
            var uiEquipment = _pool.Get();
            return uiEquipment;
        }

        public virtual void Release(TItem item)
        {
            _pool.Release(item);
        }

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

        public bool TryFocus()
        {
            Interactable = false;
            var firstChild = GetComponentInChildren<TItem>();
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