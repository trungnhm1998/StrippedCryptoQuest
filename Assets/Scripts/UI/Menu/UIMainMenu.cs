using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        public static event Action BackToNavigation;
        public static void OnBackToNavigation() => BackToNavigation?.Invoke();
        public static event Action<int> FocusTab;
        public static void OnFocusTab(int tabIndex) => FocusTab?.Invoke(tabIndex);
        [SerializeField] private TabManager _tabNavigation;
        [SerializeField] private int _defaultTabToOpen;

        private void OnEnable()
        {
            _tabNavigation.Interactable = true;
            _tabNavigation.OpenTab(_defaultTabToOpen);
            _tabNavigation.OpeningTab += DisableTabNavigation;
            FocusTab += _tabNavigation.OpenTab;
        }

        private void OnDisable()
        {
            _tabNavigation.Interactable = false;
            _tabNavigation.OpeningTab -= DisableTabNavigation;
            BackToNavigation -= EnableTabNavigation;
            FocusTab -= _tabNavigation.OpenTab;
        }

        private void DisableTabNavigation(UITabButton uiTabButton)
        {
            _tabNavigation.Interactable = false;
            foreach (var tab in _tabNavigation.Tabs)
            {
                tab.Interactable = false;
                tab.GetComponent<UITabFocus>().UnFocus();
            }

            uiTabButton.GetComponent<UITabFocus>().Focus();
            BackToNavigation += EnableTabNavigation;
        }

        private void EnableTabNavigation()
        {
            BackToNavigation -= EnableTabNavigation;
            foreach (var tab in _tabNavigation.Tabs) tab.GetComponent<UITabFocus>().Focus();
            _tabNavigation.Interactable = true;
        }
    }
}