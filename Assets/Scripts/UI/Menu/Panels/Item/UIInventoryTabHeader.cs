using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIInventoryTabHeader : MonoBehaviour
    {
        public event Action<UsableTypeSO> OpeningTab;
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

        private void OpenTab(UsableTypeSO consumableType)
        {
            HighlightTab(consumableType);
            OpeningTab?.Invoke(consumableType);
        }
        
        public void HighlightTab(UsableTypeSO type)
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