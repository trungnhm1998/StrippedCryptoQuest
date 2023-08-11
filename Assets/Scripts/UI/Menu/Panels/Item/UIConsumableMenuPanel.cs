using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using FSM;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Item Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UIConsumableMenuPanel : UIMenuPanel
    {
        [SerializeField] private UIInventoryTabHeader _inventoryTabHeader;
        [SerializeField] private UIConsumables[] _itemLists;
        [SerializeField] private LocalizeStringEvent _localizeDescription;

        private Dictionary<UsableTypeSO, UIConsumables> _itemListCache = new();
        private UIConsumables _currentConsumables;

        private void Awake()
        {
            foreach (var itemList in _itemLists)
            {
                _itemListCache.Add(itemList.Type, itemList);
            }
        }

        /// <summary>
        /// Return the specific state machine for this panel.
        /// </summary>
        /// <param name="menuManager"></param>
        /// <returns>The <see cref="ItemMenuStateMachine"/> which derived
        /// <see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> derived
        /// from <see cref="StateMachine"/> which also derived from <see cref="StateBase"/></returns>
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new ItemMenuStateMachine(this);
        }

        private void OnEnable()
        {
            _inventoryTabHeader.OpeningTab += ShowItemsWithType;
            UIConsumableItem.Inspecting += InspectingItem;
        }

        private void OnDisable()
        {
            _inventoryTabHeader.OpeningTab -= ShowItemsWithType;
            UIConsumableItem.Inspecting -= InspectingItem;
        }

        private void InspectingItem(UsableInfo item)
        {
            _localizeDescription.StringReference = item.Description;
        }


        private void ShowItemsWithType(UsableTypeSO itemType)
        {
            if (_currentConsumables) _currentConsumables.Hide();
            _currentConsumables = _itemListCache[itemType];
            _currentConsumables.Show();
        }
    }
}