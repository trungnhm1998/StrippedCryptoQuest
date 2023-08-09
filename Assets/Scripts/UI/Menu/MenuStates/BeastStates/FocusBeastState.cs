using CryptoQuest.UI.Menu.Panels.Beast;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.BeastStates
{
    public class FocusBeastState : BeastStateBase
    {
        public FocusBeastState(UIBeastMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(BeastPanel.TypeSO);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(BeastPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(BeastMenuStateMachine.NavBeast);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(BeastMenuStateMachine.Beast);
        }
    }
}