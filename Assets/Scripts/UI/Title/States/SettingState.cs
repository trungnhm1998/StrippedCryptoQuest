using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public class SettingState : InputStateBase
    {
        private UIOptionPanel _optionPanel;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            stateMachine.TryGetComponentInChildren(out _optionPanel);
            _optionPanel?.gameObject?.SetActive(true);
            _optionPanel?.InitOptionPanel();
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            _optionPanel?.gameObject?.SetActive(false);
        }

        public override void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            StateMachine.ChangeState(new StartGameState());
        }
    }
}