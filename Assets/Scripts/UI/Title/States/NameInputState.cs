using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public class NameInputState : InputStateBase
    {
        private UINamingPanel _namingPanel;
        private TitleStateMachine _stateMachine;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            _stateMachine = stateMachine;
            stateMachine.TryGetComponentInChildren(out _namingPanel);
            _namingPanel.gameObject.SetActive(true);
            _namingPanel.ConfirmButton.onClick.AddListener(OnNameInputConfirm);
            _namingPanel.NameInput.Select();
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            _namingPanel.gameObject.SetActive(false);
            _namingPanel.ConfirmButton.onClick.RemoveListener(OnNameInputConfirm);
        }

        private void OnNameInputConfirm() => _stateMachine.ChangeState(new NameConfirmation());

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

        public override void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnNavigate(context.ReadValue<Vector2>());
        }

        public override void OnCancel(InputAction.CallbackContext context)
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject;
            var nameInput = _namingPanel.NameInput;
            if (context.performed && currentSelected != nameInput.gameObject)
                _stateMachine.ChangeState(new StartGameState());
        }
    }
}