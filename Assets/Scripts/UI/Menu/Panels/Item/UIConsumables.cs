using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.System;
using PolyAndCode.UI;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumables : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        public event Action<UsableInfo> Inspecting;

        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private GameObject _content;
        [field: SerializeField] public UsableTypeSO Type { get; private set; }
        [field: SerializeField] public ServiceProvider ServiceProvider { get; private set; }
        private readonly List<UIConsumableItem> _uiConsumables = new();
        private UIConsumableItem _currentInspectingItem;
        private List<UsableInfo> _usableItems = new();

        private bool _interactable;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                foreach (var consumableButton in _uiConsumables)
                {
                    consumableButton.Interactable = value;
                }
            }
        }

        private void Awake()
        {
            SetConsumableUI();
            _recyclableScrollRect.Initialize(this);
        }

        private void SetConsumableUI()
        {
            foreach (var item in ServiceProvider.Inventory.UsableItems)
            {
                if (item.Data.UsableTypeSO == Type)
                    _usableItems.Add(item);
            }
        }

        #region PLUGINS

        public int GetItemCount()
        {
            return _usableItems.Count;
        }


        public void SetCell(ICell cell, int index)
        {
            var item = cell as UIConsumableItem;
            if (item == null)
            {
                Debug.Log("Cell is not UIConsumableItem");
                return;
            }

            _uiConsumables.Add(item);
            item.Init(_usableItems[index]);
            item.Inspecting += OnInspecting;
            if (_currentInspectingItem != null)
            {
                _currentInspectingItem.Inspect();
                return;
            }

            if (_showing && index == 0)
            {
                _currentInspectingItem = item;
                item.Inspect();
            }
        }

        private bool _isPreviousHidden = true;

        private void OnDisable()
        {
            _isPreviousHidden = true;
        }

        private void OnDestroy()
        {
            foreach (var item in _uiConsumables)
            {
                item.Inspecting -= OnInspecting;
            }
        }

        private void OnInspecting(UIConsumableItem consumableUI)
        {
            _currentInspectingItem = consumableUI;
            Inspecting?.Invoke(consumableUI.Consumable);
        }

        #endregion

        public void Hide()
        {
            _content.SetActive(false);
        }

        private bool _showing;

        public void Show()
        {
            EnsureContentIsVisible();

            if (_isPreviousHidden)
            {
                _isPreviousHidden = false;
                DeselectCurrentInspectingItem();
            }

            SetDefaultInspectingItem();

            InspectCurrentItem();
        }

        #region Over engineered

        private void EnsureContentIsVisible()
        {
            _content.SetActive(true);
            _showing = true;
        }

        private void DeselectCurrentInspectingItem()
        {
            if (!_currentInspectingItem) return;
            _currentInspectingItem = null;
        }

        private void SetDefaultInspectingItem()
        {
            if (_currentInspectingItem == null && _uiConsumables.Count > 0)
            {
                _currentInspectingItem = _uiConsumables[0];
            }
        }

        private void InspectCurrentItem()
        {
            if (_currentInspectingItem) _currentInspectingItem.Inspect();
        }

        #endregion
    }
}