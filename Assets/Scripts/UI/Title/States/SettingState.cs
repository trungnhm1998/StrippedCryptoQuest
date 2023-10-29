using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public class SettingState : InputStateBase
    {
        private UIOptionPanel _optionPanel;
        private TitleStateMachine _stateMachine;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            // _startGamePanelController = startGamePanelController;
            // _optionPanel = startGamePanelController.UIOptionPanel;
            // _inputActions.asset.FindActionMap("Title").Enable();
            // _optionPanel.gameObject.SetActive(true);
            // _optionPanel.InitOptionPanel();
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            _stateMachine = stateMachine;
            _optionPanel.gameObject.SetActive(false);
        }

        public override void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            _stateMachine.ChangeState(new StartGameState());
        }
    }
}