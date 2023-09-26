using CryptoQuest.Input;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class SocialLoginFailedState : IState, TitleInputActions.ITitleActions
    {
        private TitlePanelController _titlePanelController;
        private UISocialPanel _socialPanel;
        private TitleInputActions _inputActions;

        public SocialLoginFailedState(TitlePanelController titlePanelController)
        {
            _titlePanelController = titlePanelController;
            _socialPanel = titlePanelController.SocialPanel;
            _inputActions = new TitleInputActions();
            _inputActions.Title.SetCallbacks(this);
        }

        public void OnEnter()
        {
            _inputActions.asset.FindActionMap("Title").Enable();
            _socialPanel.LoginFailedPanel.SetActive(true);
        }

        public void OnExit()
        {
            _inputActions.asset.FindActionMap("Title").Disable();
            _socialPanel.LoginFailedPanel.SetActive(false);
        }

        private void OnCancel()
        {
            _titlePanelController.ChangeState(new TitleState(_titlePanelController));
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