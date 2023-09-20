using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumables : MonoBehaviour
    {
        public event Action<ConsumableInfo> Inspecting;

        [SerializeField] private UIConsumableItem _prefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _content;
        [field: SerializeField] public ConsumableType Type { get; private set; }
        [field: SerializeField] public ServiceProvider ServiceProvider { get; private set; }
        private readonly List<UIConsumableItem> _uiConsumables = new();
        private UIConsumableItem _currentInspectingItem;

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

        private void OnEnable()
        {
            CleanUpScrollView();
            SetConsumableUI();
        }

        private void CleanUpScrollView()
        {
            foreach (var item in _uiConsumables)
            {
                item.Inspecting -= OnInspecting;
                Destroy(item.gameObject);
            }

            _uiConsumables.Clear();
        }

        private void SetConsumableUI()
        {
            foreach (var item in ServiceProvider.Inventory.Consumables)
            {
                if (item.Data.consumableType == Type)
                    CreateItem(item);
            }

            SetDefaultInspectingItem();
            InspectCurrentItem();
        }

        private void CreateItem(ConsumableInfo consumable)
        {
            var item = Instantiate(_prefab, _scrollRect.content);
            _uiConsumables.Add(item);
            item.Init(consumable);
            item.Inspecting += OnInspecting;
            if (_currentInspectingItem != null)
            {
                _currentInspectingItem.Inspect();
            }
        }

        private bool _isPreviousHidden = true;

        private void OnDisable()
        {
            _isPreviousHidden = true;
        }

        private void OnInspecting(UIConsumableItem consumableUI)
        {
            _currentInspectingItem = consumableUI;
            Inspecting?.Invoke(consumableUI.Consumable);
        }

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
            var firstButton = _scrollRect.content.GetComponentInChildren<Button>();
            if (firstButton != null) firstButton.Select();
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