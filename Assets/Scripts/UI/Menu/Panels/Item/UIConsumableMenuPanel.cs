using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using FSM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Item Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIConsumableMenuPanel : UIMenuPanel
    {
        public event Action ItemConsumed;
        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;
        [SerializeField] private UIInventoryTabHeader _inventoryTabHeader;
        [SerializeField] private UIConsumables[] _itemLists;
        [SerializeField] private LocalizeStringEvent _localizeDescription;

        private readonly Dictionary<ConsumableType, int> _itemListCache = new();
        private UIConsumableItem _interactingUI;
        public ConsumableInfo ConsumingItem => _interactingUI.Consumable;
        private UIConsumables _currentConsumables;
        private Text _description;

        private int CurrentTabIndex
        {
            get => _currentTabIndex;
            set
            {
                if (value < 0)
                {
                    _currentTabIndex = _itemLists.Length - 1;
                }
                else if (value >= _itemLists.Length)
                {
                    _currentTabIndex = 0;
                }
                else
                {
                    _currentTabIndex = value;
                }
            }
        }

        private int _currentTabIndex;

        private void Awake()
        {
            _itemConsumedEvent.EventRaised += OnItemConsumed;
            _description = _localizeDescription.GetComponent<Text>();
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                _itemListCache.Add(itemList.Type, index);
            }
        }

        public ItemMenuStateMachine StateMachine { get; set; }

        /// <summary>
        /// Return the specific state machine for this panel.
        /// </summary>
        /// <param name="menuManager"></param>
        /// <returns>The <see cref="ItemMenuStateMachine"/> which derived
        /// <see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> derived
        /// from <see cref="StateMachine"/> which also derived from <see cref="StateBase"/></returns>
        public override StateBase<string> GetPanelState(MenuManager menuManager) =>
            StateMachine ??= new ItemMenuStateMachine(this);

        private void Start()
        {
            _inventoryTabHeader.OpeningTab += ShowItemsWithType;
            UIConsumables.Inspecting += InspectingItem;
            UIConsumableItem.Using += UseItem;
        }

        private bool _previouslyHidden = true;
        private bool _interactable = false;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                if (_currentConsumables) _currentConsumables.Interactable = _interactable;

                // TODO: BAD CODE
                if (_interactable && _interactingUI)
                {
                    EventSystem.current.SetSelectedGameObject(_interactingUI.gameObject);
                }
            }
        }

        private void OnEnable() => _previouslyHidden = true;

        protected override void OnShow()
        {
            if (!_previouslyHidden) return;
            _previouslyHidden = false;
            ShowItemsWithType(0);
        }

        protected override void OnHide() => _previouslyHidden = true;

        private void OnDestroy()
        {
            _itemConsumedEvent.EventRaised -= OnItemConsumed;
            _inventoryTabHeader.OpeningTab -= ShowItemsWithType;
            UIConsumables.Inspecting += InspectingItem;
            UIConsumableItem.Using -= UseItem;
        }

        private void OnItemConsumed(ConsumableInfo consumable)
            => ItemConsumed?.Invoke();

        private void UseItem(UIConsumableItem selectedConsumableItem)
        {
            _interactingUI = selectedConsumableItem; // to refocus the item when the item usage menu is closed
            StateMachine.RequestStateChange(ItemMenuStateMachine.ConsumingItem);
        }

        private void InspectingItem(ConsumableInfo item)
        {
            _localizeDescription.StringReference = item.Description;
        }

        private void ShowItemsWithType(ConsumableType itemType)
        {
            var index = _itemListCache[itemType];
            ShowItemsWithType(index);
        }

        private void ShowItemsWithType(int tabIndex)
        {
            if (_currentConsumables) _currentConsumables.Hide();
            CurrentTabIndex = tabIndex;
            _inventoryTabHeader.HighlightTab(_itemLists[tabIndex].Type);
            _currentConsumables = _itemLists[tabIndex];
            _currentConsumables.Show();
        }

        public void ChangeTab(float direction)
        {
            _description.text = null;
            CurrentTabIndex += (int)direction;
            ShowItemsWithType(CurrentTabIndex);
        }
    }
}