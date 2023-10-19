using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    /// <summary>
    /// Render consumables in <see cref="InventorySO"/> filter using <see cref="Type"/>
    /// </summary>
    public class UIConsumables : MonoBehaviour
    {
        [SerializeField] private UIConsumableItem _prefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _content;
        [field: SerializeField] public EConsumableType Type { get; private set; }
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

                if (_interactable == false) return;
                UpdateSelectingItemIfLastWereNull();
                InspectCurrentItem();
            }
        }

        private void OnEnable()
        {
            _currentInspectingItem = null;
            ConsumableInfo.QuantityReduced += UpdateUI;
        }

        private void OnDisable()
        {
            ConsumableInfo.QuantityReduced -= UpdateUI;
            UIConsumableItem.Inspecting -= SaveInspectingItemToSelectLater;
        }

        private void UpdateUI(ConsumableInfo consumable)
        {
            if (!consumable.IsValid() || _currentInspectingItem == null
                || !_currentInspectingItem.Consumable.IsValid()) return;

            if (consumable.Quantity <= 0)
            {
                Destroy(_currentInspectingItem.gameObject);
                return;
            }
            
            _currentInspectingItem.SetQuantityText(consumable);
        }

        private void SaveInspectingItemToSelectLater(UIConsumableItem item)
            => _currentInspectingItem = item;

        public void Hide()
        {
            UIConsumableItem.Inspecting -= SaveInspectingItemToSelectLater;
            _content.SetActive(false);
        }

        public void Show()
        {
            UIConsumableItem.Inspecting += SaveInspectingItemToSelectLater;
            _content.SetActive(true);
            CleanUpScrollView();
            RenderConsumables();
            UpdateSelectingItem();
            InspectCurrentItem();
        }

        private void CleanUpScrollView()
        {
            foreach (var item in _uiConsumables)
                Destroy(item.gameObject);

            _uiConsumables.Clear();
        }

        private void RenderConsumables()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            foreach (var item in inventory.Consumables)
            {
                if (item.Data.ConsumableType == Type)
                    CreateItem(item);
            }
        }

        private void CreateItem(ConsumableInfo consumable)
        {
            var item = Instantiate(_prefab, _scrollRect.content);
            _uiConsumables.Add(item);
            item.Init(consumable);
        }

        private void UpdateSelectingItemIfLastWereNull()
        {
            if (_currentInspectingItem == null)
            {
                UpdateSelectingItem();
            }
        }

        private void UpdateSelectingItem()
        {
            if(_uiConsumables.Count > 0)
            {
                _currentInspectingItem = _uiConsumables[0];
            }    
        }    

        private void InspectCurrentItem()
        {
            if (_currentInspectingItem) _currentInspectingItem.Inspect();
        }
    }
}