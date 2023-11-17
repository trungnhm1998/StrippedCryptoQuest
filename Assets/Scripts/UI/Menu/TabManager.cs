using System;
using CryptoQuest.Input;
using Input;
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
        public event Action<UITabButton> OpeningTab;
        [SerializeField] private InputMediatorSO _inputMediator;
        private UITabButton[] _tabs;
        public UITabButton[] Tabs => _tabs ??= GetComponentsInChildren<UITabButton>(true);
        private bool _interactable = true;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                foreach (var tab in Tabs) tab.Interactable = value;
                if (!value) return;
                Tabs[CurrentSelectedIndex].Select();
            }
        }

        private UITabButton _currentSelectedTab;

        private void Awake()
        {
            foreach (var tab in Tabs)
            {
                tab.Pressed += OnOpenTab;
                tab.Selected += CacheCurrentSelectedAndEnablePanel;
            }
        }

        private void OnDestroy()
        {
            foreach (var tab in Tabs)
            {
                tab.Pressed -= OnOpenTab;
                tab.Selected -= CacheCurrentSelectedAndEnablePanel;
            }
        }

        private void OnEnable()
        {
            _inputMediator.TabChangeEvent += ChangeTab;
            SelectTab(0);
        }

        private void OnDisable()
        {
            _inputMediator.TabChangeEvent -= ChangeTab;
        }

        private int _currentSelectedIndex;

        private int CurrentSelectedIndex
        {
            get => _currentSelectedIndex;
            set
            {
                _currentSelectedIndex = value;
                _currentSelectedIndex = Math.Clamp(_currentSelectedIndex, 0, Tabs.Length - 1);
            }
        }

        private UITabButton _openingTab = default;

        private void OnOpenTab(UITabButton tab)
        {
            if (tab == _openingTab)
            {
                OpeningTab?.Invoke(tab);
                return;
            }
            if (_openingTab != null) _openingTab.ManagedPanel.SetActive(false);
            if (tab.ManagedPanel.activeSelf == false) tab.ManagedPanel.SetActive(true);
            _openingTab = tab;
            OpeningTab?.Invoke(tab);
        }

        public void OpenTab(int tabIndex)
        {
            CurrentSelectedIndex = tabIndex;
            var uiTabButton = Tabs[CurrentSelectedIndex];
            uiTabButton.Select();
            OnOpenTab(uiTabButton);
        }

        private void ChangeTab(float direction)
        {
            if (!_interactable) return;
            CurrentSelectedIndex += (int)direction;
            Tabs[CurrentSelectedIndex].Select();
        }

        public void SelectTab(int tabIndex)
        {
            CurrentSelectedIndex = tabIndex;
            Tabs[CurrentSelectedIndex].Select();
        }

        private void CacheCurrentSelectedAndEnablePanel(UITabButton tabButton)
        {
            for (var index = 0; index < Tabs.Length; index++)
            {
                var tab = Tabs[index];
                if (tab != tabButton) continue;
                CurrentSelectedIndex = index;
                return;
            }
        }
    }
}