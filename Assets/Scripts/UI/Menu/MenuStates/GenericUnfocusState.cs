using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates
{
    [Obsolete]
    public class GenericUnfocusState : MenuStateBase
    {
        private string _someState;

        public GenericUnfocusState(string someState)
        {
            _someState = someState;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightLastFocusHeader();
        }

        public override void OnExit()
        {
            base.OnExit();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightLastFocusHeader(true);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(_someState);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MainMenuContext.CloseMainMenu();
        }

        public override void ChangeTab(float direction)
        {
            base.ChangeTab(direction);
            NavigationBar.ChangeTab(direction);
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            NavigationBar.ChangeTab(direction.x);
        }
    }
}