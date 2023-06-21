using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    // TODO: Move action maps to separate scriptable objects
    public class InputMediator : ScriptableObject, InputActions.IMapGameplayActions, InputActions.IMenusActions
    {
        public event UnityAction MenuConfirmClicked;

        private InputActions _inputActions;

        private void OnEnable()
        {
            CreateInputInstanceIfNeeded();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        #region Main

        private void DisableAllInput()
        {
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

        #endregion

        #region MapGameplayActions

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
        }

        #endregion

        #region MenuActions

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuConfirmClicked?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        #endregion
    }
}