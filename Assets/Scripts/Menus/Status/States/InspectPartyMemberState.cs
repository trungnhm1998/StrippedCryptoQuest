using CryptoQuest.Menus.Status.UI;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States
{
    public class InspectPartyMemberState : StatusStateBase
    {
        public InspectPartyMemberState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += HandleCancel;
            StatusPanel.CharacterPanel.InspectCharacter(StatusPanel.CharacterPanel.CurrentIndex);
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= HandleCancel;
        }

        private void HandleCancel() => UIMainMenu.OnBackToNavigation();

        private void HandleNavigate(Vector2 direction)
        {
            StatusPanel.CharacterPanel.ChangeCharacter(direction.x);
        }
    }
}