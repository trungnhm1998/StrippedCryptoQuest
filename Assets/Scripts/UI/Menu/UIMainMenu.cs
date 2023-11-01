using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        public static event Action BackToNavigation;
        public static void OnBackToNavigation() => BackToNavigation?.Invoke();
        [SerializeField] private TabManager _tabNavigation;
        [SerializeField] private int _defaultTabToOpen;

        private void OnEnable()
        {
            _tabNavigation.Interactable = true;
            _tabNavigation.OpenTab(_defaultTabToOpen);
            _tabNavigation.OpeningTab += DisableTabNavigation;
        }

        private void OnDisable()
        {
            _tabNavigation.Interactable = false;
            _tabNavigation.OpeningTab -= DisableTabNavigation;
            BackToNavigation -= EnableTabNavigation;
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