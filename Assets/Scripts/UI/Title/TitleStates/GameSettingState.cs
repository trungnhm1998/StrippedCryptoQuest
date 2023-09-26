using CryptoQuest.Input;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class GameSettingState : IState, TitleInputActions.ITitleActions
    {
        private StartGamePanelController _startGamePanelController;
        private UIOptionPanel _optionPanel;
        private TitleInputActions _inputActions;

        public GameSettingState(StartGamePanelController startGamePanelController)
        {
            _startGamePanelController = startGamePanelController;
            _optionPanel = startGamePanelController.UIOptionPanel;
            _inputActions = new TitleInputActions();
            _inputActions.Title.SetCallbacks(this);
        }

        public void OnEnter()
        {
            _inputActions.asset.FindActionMap("Title").Enable();
            _optionPanel.gameObject.SetActive(true);
            _optionPanel.InitOptionPanel();
        }

        public void OnExit()
        {
            _optionPanel.gameObject.SetActive(false);
        }

        private void OnCancel()
        {
            _startGamePanelController.ChangeState(new StartGameState(_startGamePanelController));
        }

        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnCancel();
        }

        public void OnConfirm(InputAction.CallbackContext context) { }
    }
}