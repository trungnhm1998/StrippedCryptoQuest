using CryptoQuest.UI.Menu.Panels.Beast;

namespace CryptoQuest.UI.Menu.MenuStates.BeastStates
{
    public class UnFocusBeastState : BeastStateBase
    {
        public UnFocusBeastState(UIBeastMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(BeastPanel.TypeSO, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(BeastPanel.TypeSO);
        }
        
        public override void Interact()
        {
            base.Interact();
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