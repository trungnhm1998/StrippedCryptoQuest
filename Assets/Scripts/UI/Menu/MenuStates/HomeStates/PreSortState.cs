using System;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class PreSortState : HomeStateBase
    {
        private UIHomeMenuSortCharacter _sortMode;

        public PreSortState(UIHomeMenu panel) : base(panel)
        {
            _sortMode = panel.SortMode;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(HomePanel.TypeSO);
            _sortMode.SelectedEvent += StartSorting;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(HomePanel.TypeSO, true);
            _sortMode.DeInit();
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.Overview);
        }

        private void StartSorting()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.Sort);
        }

        public override void OnExit()
        {
            base.OnExit();
            _sortMode.DeInit();
            _sortMode.SelectedEvent -= StartSorting;
        }
    }
}