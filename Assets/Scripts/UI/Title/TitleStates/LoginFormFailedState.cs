using CryptoQuest.Input;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class LoginFormFailedState : IState, TitleInputActions.ITitleActions
    {
        private MailLoginController _mailLoginController;
        private UISignInPanel _signInPanel;
        private TitleInputActions _inputActions;

        public LoginFormFailedState(MailLoginController mailLoginController)
        {
            _mailLoginController = mailLoginController;
            _signInPanel = _mailLoginController.UISignInPanel;
            _inputActions = new TitleInputActions();
            _inputActions.Title.SetCallbacks(this);
        }

        public void OnEnter()
        {
            _inputActions.asset.FindActionMap("Title").Enable();
            _signInPanel.LoginFailedPanel.SetActive(true);
        }

        public void OnExit()
        {
            _inputActions.asset.FindActionMap("Title").Disable();
            _signInPanel.LoginFailedPanel.SetActive(false);
        }

        private void OnCancel()
        {
            _mailLoginController.ChangeState(new MailLoginState(_mailLoginController));
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