using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class UnFocusStatusState : StatusStateBase
    {
        public UnFocusStatusState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO);
        }
        
        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.Equipment);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
        }

        public override void ChangeTab(float direction)
        {
            base.ChangeTab(direction);
            NavigationBar.ChangeTab(direction);
        }
    }
}