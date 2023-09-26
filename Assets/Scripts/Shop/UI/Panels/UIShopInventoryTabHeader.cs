using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.Panels.Item;
using CryptoQuest.Shop.UI.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopInventoryTabHeader : MonoBehaviour
    {
        public event Action<ShopStateSO> OpeningTab;
        [SerializeField] private UIShopInventoryTabButton[] _tabButtons;

        private void OnEnable()
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.Clicked += OpenTab;
            }
        }

        private void OnDisable()
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.Clicked -= OpenTab;
            }
        }

        private void OpenTab(ShopStateSO consumableType)
        {
            HighlightTab(consumableType);
            OpeningTab?.Invoke(consumableType);
        }

        public void HighlightTab(ShopStateSO type)
        {
            foreach (var tabButton in _tabButtons)
            {
                tabButton.SetHighlight(tabButton.ConsumableType == type);
            }
        }
    }
}
