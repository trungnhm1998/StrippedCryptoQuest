using System;
using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu
{
    public class UINavigationBar : MonoBehaviour
    {
        public event Action<MenuTypeSO> MenuChanged;
        [SerializeField] private MenuSelectionHandler _handler;
        [SerializeField] private UIHeaderButton _defaultSelect;
        [SerializeField] private List<UIHeaderButton> _navBarButtons = new();

        private int _currentSelectedIndex;

        private void OnEnable()
        {
            RegisterNavBarButtonEvents();
            _handler.UpdateSelection(_defaultSelect.gameObject);
        }

        private void OnDisable()
        {
            UnregisterNavBarButtonEvents();
        }

        private void RegisterNavBarButtonEvents()
        {
            foreach (var button in _navBarButtons)
            {
                button.Pressed += OnHeaderPressed;
                button.Selected += NavBarButtonSelect;
            }
        }

        private void UnregisterNavBarButtonEvents()
        {
            foreach (var button in _navBarButtons)
            {
                button.Pressed -= OnHeaderPressed;
                button.Selected -= NavBarButtonSelect;
            }
        }

        public UIHeaderButton _lastSelectButton;

        private void NavBarButtonSelect(UIHeaderButton arg0)
        {
            _lastSelectButton = arg0;
        }

        private void OnHeaderPressed(MenuTypeSO menuTypeSO)
        {
            MenuChanged?.Invoke(menuTypeSO);
        }

        /// <summary>
        /// Highlight the button but it doesn't mean it's interactable
        /// </summary>
        /// <param name="status">which type this header button, this SO might contains the localized string for header</param>
        /// <param name="selecting">will this button be select even thought the button component could be disabled</param>
        public void HighlightHeader(MenuTypeSO status, bool selecting = false)
        {
            var headerButton = _navBarButtons[(int)status.Type];
            headerButton.Focus();

            if (selecting)
                headerButton.Select();
        }
        
        public void HighlightLastFocusHeader(bool selecting = false)
        {
            HighlightHeader(_lastSelectButton.TypeSO);
            if (selecting)
                _lastSelectButton.Select();
        }

        public void SetActive(bool isActive)
        {
            EventSystem.current?.SetSelectedGameObject(null);
            foreach (var button in _navBarButtons)
            {
                button.enabled = isActive;
                if (isActive)
                {
                    button.Focus();
                }
                else
                {
                    button.UnFocus();
                }
            }
        }

        private int CurrentSelectedIndex
        {
            get => _currentSelectedIndex;
            set
            {
                if (value >= _navBarButtons.Count)
                {
                    _currentSelectedIndex = 0;
                }
                else if (value < 0)
                {
                    _currentSelectedIndex = _navBarButtons.Count - 1;
                }
                else
                {
                    _currentSelectedIndex = value;
                }
            }
        }

        private void NextHeader()
        {
            UpdateMouseSelection();
            CurrentSelectedIndex++;
            _navBarButtons[CurrentSelectedIndex].Select();
        }

        private void PreviousHeader()
        {
            UpdateMouseSelection();
            CurrentSelectedIndex--;
            _navBarButtons[CurrentSelectedIndex].Select();
        }

        private void UpdateMouseSelection()
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<UIHeaderButton>();
            if (currentSelected != null && currentSelected != _navBarButtons[_currentSelectedIndex])
            {
                _currentSelectedIndex = _navBarButtons.IndexOf(currentSelected);
            }
        }

        public void ChangeTab(float direction)
        {
            switch (direction)
            {
                case >= 1:
                    NextHeader();
                    break;
                case <= -1:
                    PreviousHeader();
                    break;
            }
        }
    }
}