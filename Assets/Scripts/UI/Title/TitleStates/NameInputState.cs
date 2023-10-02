using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class NameInputState : IState, TitleInputActions.ITitleActions
    {
        private StartGamePanelController _startGamePanelController;
        private UINamingPanel _namingPanel;
        private TitleInputActions _inputActions;

        public NameInputState(StartGamePanelController startGamePanelController)
        {
            _startGamePanelController = startGamePanelController;
            _namingPanel = startGamePanelController.UINamingPanel;
            _inputActions = new TitleInputActions();
            _inputActions.Title.SetCallbacks(this);
        }

        public void OnEnter()
        {
            _inputActions.asset.FindActionMap("Title").Enable();
            _namingPanel.gameObject.SetActive(true);
            _namingPanel.ConfirmButton.onClick.AddListener(OnNameInputConfirm);
            _namingPanel.NameInput.Select();
        }

        public void OnExit()
        {
            _inputActions.asset.FindActionMap("Title").Disable();
            _namingPanel.gameObject.SetActive(false);
            _namingPanel.ConfirmButton.onClick.RemoveListener(OnNameInputConfirm);
        }

        private void OnCancel()
        {
            _startGamePanelController.ChangeState(new StartGameState(_startGamePanelController));
        }

        private void OnNameInputConfirm()
        {
            if (_namingPanel.IsInputValid())
            {
                _namingPanel.SetTempName(_namingPanel.NameInput.text);
                _startGamePanelController.ChangeState(new NameConfirmState(_startGamePanelController));
            }
        }

        private void OnNavigate(Vector2 direction)
        {
            if (direction.y == 0) return;
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            var nameInput = _namingPanel.NameInput;
            var confirmButton = _namingPanel.ConfirmButton;
            if (currentSelected != nameInput.gameObject)
            {
                nameInput.Select();
                return;
            }

            confirmButton.Select();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnNavigate(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnCancel();
        }

        public void OnConfirm(InputAction.CallbackContext context) { }
    }
}