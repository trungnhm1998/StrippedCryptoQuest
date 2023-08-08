using CryptoQuest.UI.Menu.Panels.Option;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class FocusOptionState : OptionStateBase
    {
        public FocusOptionState(UIOptionMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(OptionPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(OptionMenuStateMachine.NavOption);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(OptionMenuStateMachine.Option);
        }
    }
}