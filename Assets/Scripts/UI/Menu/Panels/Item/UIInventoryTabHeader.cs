using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIInventoryTabHeader : MonoBehaviour
    {
        public event Action<UsableTypeSO> OpeningTab;
        [SerializeField] private UIInventoryTabButton _defaultSelectedTabButton;
        [SerializeField] private UIInventoryTabButton[] _tabButtons;

        private void OnEnable()
        {
            OpenTab(_defaultSelectedTabButton.ConsumableType);

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
            foreach (var tabButton in _tabButtons)
            {
                if (tabButton.ConsumableType != consumableType)
                    tabButton.UnHighlight();
                else
                    tabButton.Highlight(); // a little bit redundant
            }
            OpeningTab?.Invoke(consumableType);
        }
    }
}