using CryptoQuest.UI.Menu.Panels.Status;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class InspectPartyMemberState : StatusStateBase
    {
        public InspectPartyMemberState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO);
            StatusPanel.CharacterPanel.InspectCharacter(StatusPanel.CharacterPanel.CurrentIndex);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.NavStatus);
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            StatusPanel.CharacterPanel.ChangeCharacter(direction.x);
        }
    }
}