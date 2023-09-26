using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class MailLoginState : IState, TitleInputActions.ITitleActions
    {
        private readonly UISignInPanel _uiSignIn;
        private TitleInputActions _inputActions;
        private MailLoginController _mailLoginController;

        public MailLoginState(MailLoginController mailLoginController)
        {
            _mailLoginController = mailLoginController;
            _uiSignIn = mailLoginController.UISignInPanel;
            _inputActions = new TitleInputActions();
            _inputActions.Title.SetCallbacks(this);
        }

        public void OnEnter()
        {
            _inputActions.asset.FindActionMap("Title").Enable();
            _uiSignIn.gameObject.SetActive(true);
            _mailLoginController.Subscribe();
            _uiSignIn.SignInButton.onClick.AddListener(OnSignInButtonPressed);
            _uiSignIn.Selectables[0].Select();
        }

        public void OnExit()
        {
            _mailLoginController.Unsubscribe();
            _inputActions.asset.FindActionMap("Title").Disable();
            _uiSignIn.SignInButton.onClick.RemoveListener(OnSignInButtonPressed);
            _uiSignIn.gameObject.SetActive(false);
        }

        private void OnSignInButtonPressed()
        {
            _mailLoginController.OnLoginFormSubmit();
        }

        private void OnSignInCancel()
        {
            _mailLoginController.ChangeState(new TitleState(_mailLoginController.TitlePanelController));
        }

        private void OnSignInMenuNavigate(Vector2 direction)
        {
            _uiSignIn.HandleDirection(direction.y);
        }


        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSignInMenuNavigate(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSignInCancel();
        }

        public void OnConfirm(InputAction.CallbackContext context) { }
    }
}