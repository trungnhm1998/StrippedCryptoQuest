using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Input;
using CryptoQuest.Item;
using CryptoQuest.Menus.Item.States;
using CryptoQuest.UI.Menu;
using IndiGames.Core.Events.ScriptableObjects;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Item.UI
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Item Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIConsumableMenuPanel : UIMenuPanelBase
    {
        [field: SerializeField]
        public InputMediatorSO Input { get; private set; }

        [field: SerializeField] public UIItemCharacterPartySlot[] HeroButtons { get; private set; }

        public UIItemCharacterPartySlot SelectingHero { get; set; }
        [field: SerializeField] public VoidEventChannelSO SingleAlliedTarget { get; private set; }
        [field: SerializeField] public VoidEventChannelSO AllAlliesTarget { get; private set; }
        [field: Header("State Context")] public event Action ItemConsumed;

        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;
        [SerializeField] private UIInventoryTabHeader _inventoryTabHeader;
        [SerializeField] private UIConsumables[] _itemLists;
        [SerializeField] private LocalizeStringEvent _localizeDescription;
        [SerializeField] private UIConsumables _currentConsumables;

        private ItemMenuStateMachine _itemMenuStateMachine;
        private readonly Dictionary<EConsumableType, int> _itemListCache = new();
        private Text _description;

        private bool _interactable;
        private int _currentTabIndex;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                _currentConsumables.Interactable = _interactable;
            }
        }

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


        private void Awake()
        {
            _itemMenuStateMachine ??= new ItemMenuStateMachine(this);

            _itemConsumedEvent.EventRaised += OnItemConsumed;
            _description = _localizeDescription.GetComponent<Text>();
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                _itemListCache.Add(itemList.Type, index);
            }

            _inventoryTabHeader.OpeningTab += ShowItemsWithType;
            UIConsumableItem.Inspecting += InspectingItem;
        }


        private void OnEnable()
        {
            _itemMenuStateMachine.Init();
        }

        private void OnDisable()
        {
            _itemMenuStateMachine.OnExit();
        }

        private void OnDestroy()
        {
            _itemConsumedEvent.EventRaised -= OnItemConsumed;
            _inventoryTabHeader.OpeningTab -= ShowItemsWithType;
            UIConsumableItem.Inspecting -= InspectingItem;
        }

        private void OnItemConsumed(ConsumableInfo consumable)
            => ItemConsumed?.Invoke();

        private void InspectingItem(UIConsumableItem item)
        {
            _localizeDescription.StringReference = item.Consumable.Description;
        }

        public void ShowItemsWithType(EConsumableType itemType)
        {
            var index = _itemListCache[itemType];
            ShowItemsWithType(index);
        }

        private void ShowItemsWithType(int tabIndex)
        {
            _currentConsumables.Hide();
            CurrentTabIndex = tabIndex;
            _inventoryTabHeader.HighlightTab(_itemLists[tabIndex].Type);
            _currentConsumables = _itemLists[tabIndex];
            _currentConsumables.Show();
        }

        public void ChangeTab(float direction)
        {
            EventSystem.current.SetSelectedGameObject(null);
            _description.text = null;
            CurrentTabIndex += (int)direction;
            ShowItemsWithType(CurrentTabIndex);
        }

        public void EnableAllHeroButtons(bool isEnabled = true)
        {
            foreach (var button in HeroButtons) button.Interactable = isEnabled;
        }
    }
}