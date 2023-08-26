using UnityEngine;
using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    /// <summary>
    /// This is the state for <see cref="StatusMenuStateMachine"/> that also defined to be a default state when
    /// enter the State Machine.
    /// </summary>
    public class FocusStatusState : StatusStateBase
    {
        public FocusStatusState(UIStatusMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO);
            StatusPanel.CharacterEquipmentsPanel.Show();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.Equipment);
        }
    }
}