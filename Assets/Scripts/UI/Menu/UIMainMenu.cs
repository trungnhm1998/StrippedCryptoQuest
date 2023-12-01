using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        public static event Action BackToNavigation;
        public static void OnBackToNavigation() => BackToNavigation?.Invoke();
        public bool IsNavigating { get; private set; }
        public static event Action<int> FocusTab;
        public static void OnFocusTab(int tabIndex) => FocusTab?.Invoke(tabIndex);
        [SerializeField] private TabManager _tabNavigation;
        [SerializeField] private int _defaultTabToOpen;

        private bool Interactable
        {
            get => _tabNavigation.Interactable;
            set => IsNavigating = _tabNavigation.Interactable = value;
        }

        private void OnEnable()
        {
            Interactable = true;
            _tabNavigation.OpenTab(_defaultTabToOpen);
            _tabNavigation.OpeningTab += DisableTabNavigation;
            FocusTab += _tabNavigation.OpenTab;

            foreach (var tab in _tabNavigation.Tabs) tab.GetComponent<UITabFocus>().Focus();
        }

        private void OnDisable()
        {
            Interactable = false;
            _tabNavigation.OpeningTab -= DisableTabNavigation;
            BackToNavigation -= EnableTabNavigation;
            FocusTab -= _tabNavigation.OpenTab;
        }

        private void DisableTabNavigation(UITabButton uiTabButton)
        {
            Interactable = false;
            foreach (var tab in _tabNavigation.Tabs)
            {
                tab.Interactable = false;
                tab.GetComponent<UITabFocus>().UnFocus();
            }

            uiTabButton.GetComponent<UITabFocus>().Focus();
            var menuPanel = uiTabButton.ManagedPanel.GetComponent<UIMenuPanelBase>();
            if (menuPanel)
                menuPanel.OnFocusing();
            BackToNavigation += EnableTabNavigation;
        }

        private void EnableTabNavigation()
        {
            BackToNavigation -= EnableTabNavigation;
            foreach (var tab in _tabNavigation.Tabs) tab.GetComponent<UITabFocus>().Focus();
            Interactable = true;
        }
    }
}