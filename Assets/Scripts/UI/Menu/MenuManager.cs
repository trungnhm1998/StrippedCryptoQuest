using System;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Serializable]
        public struct MenuMap
        {
            public MenuTypeSO TypeSO;
            public UIMenuPanel Panel;
        }

        [FormerlySerializedAs("EnableSortModeEvent")]
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO SortModeEnabledEvent;
        [SerializeField] private VoidEventChannelSO SortModeDisabledEvent;

        [Header("Game Components")]
        [SerializeField] private RectTransform _navBar;
        [SerializeField] private List<UIHeaderButton> _navBarButtons = new();
        [SerializeField] private List<MenuMap> _panels = new();

        private Dictionary<EMenuType, UIMenuPanel> _cachedPanel = new();
        private UIMenuPanel _currentActivePanel;
        private MenuTypeSO _currentActiveMenuTypeSO;

        private void OnEnable()
        {
            SortModeEnabledEvent.EventRaised += DisableButtonsInteraction;
            SortModeDisabledEvent.EventRaised += EnableButtonsInteraction;
        }

        private void OnDisable()
        {
            SortModeEnabledEvent.EventRaised -= DisableButtonsInteraction;
            SortModeDisabledEvent.EventRaised -= EnableButtonsInteraction;
        }

        private void Awake()
        {
            _cachedPanel = new();
            foreach (var menuMap in _panels)
            {
                _cachedPanel.Add(menuMap.TypeSO.Type, menuMap.Panel);
            }

            _currentActivePanel = _cachedPanel[EMenuType.Main];
        }

        private void DisableButtonsInteraction()
        {
            foreach (var button in _navBarButtons)
            {
                button.interactable = false;
            }
        }

        private void EnableButtonsInteraction()
        {
            Debug.Log($"Sort mode turn off");
            foreach (var button in _navBarButtons)
            {
                button.interactable = true;
            }
        }

        public void MainMenuBeingClosed()
        {
            foreach (var button in _navBarButtons)
            {
                button.Pressed -= MenuHeaderButtonPressed;
            }
        }

        public void ShowMainMenuPanel()
        {
            UpdateNavBarLayout();

            if (_currentActivePanel != null && _currentActivePanel.TypeSO.Type != EMenuType.Main)
            {
                HideCurrentPanel();
            }

            _currentActivePanel = _cachedPanel[EMenuType.Main];
            _currentActiveMenuTypeSO = _currentActivePanel.TypeSO;
            RegisterNavBarButtonEvents();
            ShowCurrentPanel();
        }

        private void RegisterNavBarButtonEvents()
        {
            foreach (var button in _navBarButtons)
            {
                button.Pressed += MenuHeaderButtonPressed;
            }
        }

        public void MenuHeaderButtonPressed(MenuTypeSO typeSO)
        {
            var typeToShow = typeSO.Type;
            if (_currentActivePanel != null && _currentActivePanel.TypeSO.Type == typeToShow)
            {
                return;
            }

            if (typeToShow == EMenuType.Main) return;
            
            _currentActiveMenuTypeSO = typeSO;
            
            HideCurrentPanel();
            _currentActivePanel = _cachedPanel[typeToShow];
            ShowCurrentPanel();

            EnableCurrectActiveHeaderButton(typeSO);
        }

        /// <summary>
        /// disable other buttons then the current active one
        /// hide arrow from the active button
        /// </summary>
        /// <param name="typeSO">typeSO from the pressed button</param>
        private void EnableCurrectActiveHeaderButton(MenuTypeSO typeSO)
        {
            foreach (var button in _navBarButtons)
            {
                // disable button component so it could be remove from unity event system
                button.interactable = false;
                button.Disable();
                if (button.TypeSO == typeSO)
                {
                    Debug.Log("test");
                    button.Pointer.enabled = false;
                    button.Enable();
                }
            }
        }

        private void EnableAllNavButtons()
        {
            foreach (var button in _navBarButtons)
            {
                button.interactable = true;
                button.Enable();
                button.Pointer.enabled = button.TypeSO == _currentActiveMenuTypeSO;
            }
        }
        public void UpdateNavBarLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_navBar);
        }

        private void ShowCurrentPanel()
        {
            if (_currentActivePanel == null) return;
            _currentActivePanel.Show();
            _currentActivePanel.PanelUnfocus = BackToMenuNavigationState;
        }

        private void BackToMenuNavigationState(MenuTypeSO menuTypeSO)
        {
            EnableAllNavButtons();
            _currentActivePanel = null;
        }

        private void HideCurrentPanel()
        {
            if (_currentActivePanel == null) return;
            _currentActivePanel.Hide();
        }
    }
}