using System;
using System.Collections.Generic;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using DG.Tweening;
using FSM;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Item Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIConsumableMenuPanel : UIMenuPanel
    {
        private const int END_ELEMENT = 1;

        private const string CHARACTER_NAME = "characterName";
        private const string ATTRIBUTE_NAME = "attributeName";
        private const string ATTRIBUTE_VALUE = "attributeValue";
        public event Action<UsableInfo> Inspecting;

        [SerializeField] private UIInventoryTabHeader _inventoryTabHeader;
        [SerializeField] private UIConsumables[] _itemLists;
        [SerializeField] private LocalizeStringEvent _localizeDescription;

        [Header("Logger Attribute"), SerializeField]
        private LocalizedString _loggerLocalizedString;

        [SerializeField] private LoggerAttribute _loggerAttribute;
        [SerializeField] private LocalizeStringEvent _loggerDescription;
        [SerializeField] private GameObject _loggerPanel;
        [SerializeField] private float _delayTime = 1f;

        private readonly Dictionary<UsableTypeSO, int> _itemListCache = new();
        private UIConsumableItem _usingItem;
        private UIConsumables _currentConsumables;
        private Tween _callbackTween;
        private Text _description;
        private Text _loggerText;

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
            _description = _localizeDescription.GetComponent<Text>();
            _loggerText = _loggerDescription.GetComponent<Text>();
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
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return StateMachine ??= new ItemMenuStateMachine(this);
        }

        private void Start()
        {
            _inventoryTabHeader.OpeningTab += ShowItemsWithType;
            foreach (var itemList in _itemLists)
            {
                itemList.Inspecting += InspectingItem;
            }

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

                // TODO: BADE CODE
                if (_interactable && _usingItem)
                {
                    EventSystem.current.SetSelectedGameObject(_usingItem.gameObject);
                }
            }
        }

        private void OnEnable()
        {
            _previouslyHidden = true;
            _loggerAttribute.OnAttributeChanged += SetLoggerDescription;
        }

        private void OnDisable()
        {
            _loggerAttribute.OnAttributeChanged -= SetLoggerDescription;
        }

        protected override void OnShow()
        {
            if (_previouslyHidden)
            {
                _previouslyHidden = false;
                ShowItemsWithType(0);
            }
        }

        protected override void OnHide()
        {
            _previouslyHidden = true;
        }

        private void OnDestroy()
        {
            _inventoryTabHeader.OpeningTab -= ShowItemsWithType;
            foreach (var itemList in _itemLists)
            {
                itemList.Inspecting -= InspectingItem;
            }

            UIConsumableItem.Using -= UseItem;
        }

        private void UseItem(UIConsumableItem consumable)
        {
            _usingItem = consumable;
            consumable.Use();
        }

        private void InspectingItem(UsableInfo item)
        {
            _localizeDescription.StringReference = item.Description;
            Inspecting?.Invoke(item);
        }

        private void SetLoggerDescription(AttributeSystemBehaviour attributeSystem, AttributeValue attributeValue)
        {
            _callbackTween?.Kill();
            _loggerPanel.SetActive(true);

            CharacterSpec characterSpec = attributeSystem.GetComponent<CharacterBehaviourBase>().Spec;
            string attributeName = attributeValue.Attribute.name.Split('.')[END_ELEMENT];
            string characterName = characterSpec.BackgroundInfo.name;

            _loggerLocalizedString.Add(CHARACTER_NAME, new StringVariable()
            {
                Value = characterName
            });

            _loggerLocalizedString.Add(ATTRIBUTE_NAME, new StringVariable()
            {
                Value = attributeName
            });

            _loggerLocalizedString.Add(ATTRIBUTE_VALUE, new FloatVariable()
            {
                Value = attributeValue.CurrentValue
            });

            _loggerText.text += $"{_loggerLocalizedString.GetLocalizedString()}\n";

            _callbackTween = DOVirtual.DelayedCall(_delayTime, HideLoggerDescription);
        }

        private void HideLoggerDescription()
        {
            _loggerPanel.SetActive(false);
            _loggerText.text = null;
        }

        private void ShowItemsWithType(UsableTypeSO itemType)
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