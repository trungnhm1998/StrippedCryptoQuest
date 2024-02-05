using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Menus.Beast.Sagas;
using CryptoQuest.UI.Common;
using CryptoQuest.UI.Utilities;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastList : MonoBehaviour
    {
        [SerializeField] private BeastInventorySO _beastInventory;

        [SerializeField] private BeastProvider _beastProvider;

        [Header("Scroll View Configs")] [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField] private UIBeast _prefab;
        private IObjectPool<UIBeast> _beastUIPool;

        private IObjectPool<UIBeast> BeastUIPool =>
            _beastUIPool ??= new ObjectPool<UIBeast>(OnCreate, OnGet, OnRelease, OnDestroyBeast);

        private readonly List<UIBeast> _beastUIs = new();

        private float _verticalOffset;

        private bool _interactable;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                foreach (var beastUi in _beastUIs) beastUi.Interactable = value;
            }
        }

        public bool IsValid => _beastInventory.OwnedBeasts.Count > 0;

        public void Init()
        {
            CleanUpScrollView();
            InitBeastList();
            Refresh();
        }

        public void EquipBeast(UIBeast ui)
        {
            _beastProvider.EquippingBeast =
                _beastProvider.EquippingBeast.Id != ui.Beast.Id
                    ? ui.Beast
                    : NullBeast.Instance;

            ActionDispatcher.Dispatch(new UpdateBeastEquippedAction(ui.Beast));
            Refresh();
        }

        private void Refresh()
        {
            foreach (var beastUi in _beastUIs)
                beastUi.MarkedForEquipped = beastUi.Beast.Id == _beastProvider.EquippingBeast.Id;
        }


        public void SelectFirstBeast()
        {
            _scrollRect.content.GetOrAddComponent<SelectFirstChildInList>().Select();
        }

        private void InitBeastList()
        {
            foreach (var beast in _beastInventory.OwnedBeasts)
            {
                var beastUI = BeastUIPool.Get();
                beastUI.Init(beast);
                beastUI.MarkedForEquipped = beastUI.Beast.Id == _beastProvider.EquippingBeast.Id;
            }
        }

        private void CleanUpScrollView()
        {
            _beastUIs.ForEach(ui => BeastUIPool.Release(ui));
            _beastUIs.Clear();
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