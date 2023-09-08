using System;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class PreSortState : HomeStateBase
    {
        public PreSortState(UIHomeMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(HomePanel.TypeSO);
            HomePanel.SortMode.SelectedEvent += StartSorting;
            HomePanel.SortMode.Init();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(HomePanel.TypeSO, true);
            HomePanel.SortMode.DeInit();
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.Overview);
        }

        private void StartSorting()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.Sort);
        }

        public override void OnExit()
        {
            base.OnExit();
            HomePanel.SortMode.DeInit();
            HomePanel.SortMode.SelectedEvent -= StartSorting;
        }
    }
}