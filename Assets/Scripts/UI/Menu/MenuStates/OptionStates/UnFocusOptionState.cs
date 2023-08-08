using CryptoQuest.UI.Menu.Panels.Option;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class UnFocusOptionState : OptionStateBase
    {
        public UnFocusOptionState(UIOptionMenu optionPanel) : base(optionPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO);
        }
        
        public override void Interact()
        {
            base.Interact();
            // MenuStateMachine.RequestStateChange(OptionMenuStateMachine);
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
    }
}