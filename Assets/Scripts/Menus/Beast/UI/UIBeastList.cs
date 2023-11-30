using System.Collections.Generic;
using CryptoQuest.Beast;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastList : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO beastInventory;

        [Header("Scroll View Configs")]
        [SerializeField] private ScrollRect _scrollRect;

        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;
        [SerializeField] private UIBeast _prefab;
        private IObjectPool<UIBeast> _beastUIPool;

        private IObjectPool<UIBeast> BeastUIPool =>
            _beastUIPool ??= new ObjectPool<UIBeast>(OnCreate, OnGet, OnRelease, OnDestroyBeast);

        private List<UIBeast> _beastUIs = new();

        private float _verticalOffset;

        public bool Interactable
        {
            set
            {
                foreach (var beastUi in _beastUIs) beastUi.Interactable = value;
            }
        }

        private void OnEnable()
        {
            CleanUpScrollView();
            InitBeastList();

            if (_scrollRect.content.childCount == 0) return;
            DOVirtual.DelayedCall(0,
                () => { EventSystem.current.SetSelectedGameObject(_scrollRect.content.GetChild(0).gameObject); });
        }

        private void InitBeastList()
        {
            foreach (var beast in beastInventory.OwnedBeasts)
            {
                var beastUI = BeastUIPool.Get();
                beastUI.Init(beast);
            }
        }

        private void CleanUpScrollView()
        {
            foreach (var ui in _beastUIs)
            {
                BeastUIPool.Release(ui);
            }

            _beastUIs.Clear();
        }

        private bool ShouldMoveUp => _scrollRect.content.anchoredPosition.y > _verticalOffset;

        private bool ShouldMoveDown =>
            _scrollRect.content.rect.height - _scrollRect.content.anchoredPosition.y
            > _scrollRect.viewport.rect.height + _verticalOffset / 2;

        public void DisplayNavigateArrows()
        {
            _upArrow.SetActive(ShouldMoveUp);
            _downArrow.SetActive(ShouldMoveDown);
        }

        #region Pool

        private void OnDestroyBeast(UIBeast beast) => Destroy(beast.gameObject);

        private void OnRelease(UIBeast beast) => beast.gameObject.SetActive(false);

        private void OnGet(UIBeast beast)
        {
            _beastUIs.Add(beast);
            beast.transform.SetAsLastSibling();
            beast.gameObject.SetActive(true);
        }

        private UIBeast OnCreate() => Instantiate(_prefab, _scrollRect.content);

        #endregion
    }
}