using CryptoQuest.Menus.Status.UI;

namespace CryptoQuest.Menus.Status.States
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
            StatusPanel.CharacterEquipmentsPanel.Show();
        }

        private void HandleCancel() { }

        private void Interact()
        {
            fsm.RequestStateChange(StatusMenuStateMachine.Equipment);
        }
    }
}