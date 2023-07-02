using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    // TODO: Move action map interfaces to separate scriptable objects
    public class InputMediatorSO : ScriptableObject, InputActions.IMapGameplayActions, InputActions.IMenusActions,
        InputActions.IDialoguesActions
    {
        #region Events

        #region Gameplay

        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction PauseEvent;
        public event UnityAction InteractEvent;

        #endregion

        #region Menu

        public event UnityAction MenuConfirmPressed;
        public event UnityAction MenuSubmitPressed;
        public event UnityAction MenuMouseMoveEvent;
        public event UnityAction MoveSelectionEvent;
        public event UnityAction CancelEvent;

        #endregion

        #region Dialogue

        public event UnityAction NextDialoguePressed;

        #endregion

        #endregion

        private InputActions _inputActions;

        private void OnEnable()
        {
            CreateInputInstanceIfNeeded();

            _inputActions.Disable();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        #region Main

        public void DisableAllInput()
        {
            _inputActions.Disable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Menus.Disable();
        }

        private void CreateInputInstanceIfNeeded()
        {
            if (_inputActions != null) return;

            _inputActions = new InputActions();
            _inputActions.Menus.SetCallbacks(this);
            _inputActions.MapGameplay.SetCallbacks(this);
        }

        public void EnableMenuInput()
        {
            _inputActions.Menus.Enable();
            _inputActions.MapGameplay.Disable();
        }

        public void EnableMapGameplayInput()
        {
            _inputActions.Menus.Disable();
            _inputActions.MapGameplay.Enable();
        }

        #endregion

        #region MapGameplayActions

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) InteractEvent?.Invoke();
        }

        public void OnInventory(InputAction.CallbackContext context) { }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed) PauseEvent?.Invoke();
        }

        #endregion

        #region MenuActions

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.performed) MoveSelectionEvent?.Invoke();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) MenuMouseMoveEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuConfirmPressed?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) MenuSubmitPressed?.Invoke();
        }

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
        public void OnClick(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        #endregion

        #region Dialogue

        public void OnNextDialogue(InputAction.CallbackContext context)
        {
            if (context.performed) NextDialoguePressed?.Invoke();
        }

        #endregion
    }
}