using System.Collections.Generic;
using CryptoQuest.Inventory;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Consumable;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Item.UI
{
    /// <summary>
    /// Render consumables in <see cref="EquipmentInventory"/> filter using <see cref="Type"/>
    /// </summary>
    public class UIConsumables : MonoBehaviour
    {
        [SerializeField] private ConsumableInventory _inventory;
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
            if (_currentInspectingItem != null) _currentInspectingItem.Consumable.QuantityChanged += UpdateUI;
        }

        private void OnDisable()
        {
            if (_currentInspectingItem != null) _currentInspectingItem.Consumable.QuantityChanged -= UpdateUI;
            UIConsumableItem.Inspecting -= SaveInspectingItemToSelectLater;
        }

        private void UpdateUI(ConsumableInfo consumable)
        {
            if (consumable.Data == null || _currentInspectingItem == null
                                        || _currentInspectingItem.Consumable.Data == null) return;

            if (consumable.Quantity <= 0)
            {
                _uiConsumables.Remove(_currentInspectingItem);
                Destroy(_currentInspectingItem.gameObject);
                UpdateSelectingItem();
                return;
            }

            _currentInspectingItem.SetQuantityText(consumable);
        }

        private void SaveInspectingItemToSelectLater(UIConsumableItem item)
        {
            if (_currentInspectingItem != null) _currentInspectingItem.Consumable.QuantityChanged -= UpdateUI;
            _currentInspectingItem = item;
            _currentInspectingItem.Consumable.QuantityChanged += UpdateUI;
        }

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
            foreach (var item in _inventory.Items)
            {
                if (item.Data.Type == Type)
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
            if (_uiConsumables.Count > 0)
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