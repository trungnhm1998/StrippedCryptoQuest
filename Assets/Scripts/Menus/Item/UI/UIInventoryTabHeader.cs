﻿using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIInventoryTabHeader : MonoBehaviour
    {
        public event Action<EConsumableType> OpeningTab;
        [SerializeField] private UIInventoryTabButton[] _tabButtons;

        /// <summary>
        /// Open consumable tab by default
        /// </summary>
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

        private UIInventoryTabButton _lastSelectedTabButton;

        private void OpenTab(EConsumableType consumableType)
        {
            HighlightTab(consumableType);
            OpeningTab?.Invoke(consumableType);
        }
        
        public void HighlightTab(EConsumableType type)
        {
            foreach (var tabButton in _tabButtons)
            {
                if (tabButton.ConsumableType != type)
                    tabButton.UnHighlight();
                else
                    tabButton.Highlight();
            }
        }
    }
}