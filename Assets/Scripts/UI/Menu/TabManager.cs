using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    /// <summary>
    /// Manage panel visibility based on tab selection.
    ///
    /// Make sure there is a tab always selected.
    /// </summary>
    public class TabManager : MonoBehaviour
    {
        public event Action<UITabButton> TabChanged;
        [SerializeField] private bool _selectFirstTabOnStart = false;
        private UITabButton[] _tabs;
        public UITabButton[] Tabs => _tabs;
        private UITabButton _currentSelectedTab;

        private void Awake()
        {
            _tabs = GetComponentsInChildren<UITabButton>();
            foreach (var tab in _tabs)
            {
                tab.Selected += CacheCurrentSelectedAndEnablePanel;
                tab.Deselected += DisablePanelIfNotCurrentSelected;
                tab.ManagedPanel.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var tab in _tabs)
            {
                tab.Selected -= CacheCurrentSelectedAndEnablePanel;
                tab.Deselected -= DisablePanelIfNotCurrentSelected;
            }
        }

        private void Start()
        {
            if (_selectFirstTabOnStart) SelectFirstTab();
        }

        public void SelectFirstTab()
        {
            for (var index = 0; index < _tabs.Length; index++)
            {
                var tab = _tabs[index];
                if (index == 0)
                {
                    tab.Select();
                    continue;
                }

                tab.Deselect();
            }
        }

        private UITabButton _selectedTab;

        private void CacheCurrentSelectedAndEnablePanel(UITabButton tabButton)
        {
            if (tabButton == _selectedTab) return;
            var previousSelectedTab = _selectedTab;
            _selectedTab = tabButton;
            DisablePanelIfNotCurrentSelected(previousSelectedTab);
            _selectedTab.ManagedPanel.SetActive(true);
            TabChanged?.Invoke(_selectedTab);
        }

        /// <summary>
        /// Disable panel if not current selected.
        /// Prevent deselect current selected tab.
        /// </summary>
        /// <param name="tabButton"></param>
        private void DisablePanelIfNotCurrentSelected(UITabButton tabButton)
        {
            if (tabButton == null) return;
            if (tabButton == _selectedTab) return;
            tabButton.ManagedPanel.SetActive(false);
        }
    }
}