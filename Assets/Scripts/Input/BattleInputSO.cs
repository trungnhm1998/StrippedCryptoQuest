using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    public class BattleInputSO : ScriptableObject,
        InputActions.IBattleMenuActions
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        public event UnityAction<Vector2> NavigateEvent;
        public event UnityAction ConfirmedEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction CancelEvent;

        public InputActions InputActions => _inputMediator.InputActions;

        #region Main

        // TODO: Inherit this later from base input mediator
        public void DisableAllInput()
        {
            _inputMediator.DisableAllInput();
            InputActions.BattleMenu.Disable();
        }

        public void EnableMapGameplayInput()
        {
            DisableAllInput();
            InputActions.MapGameplay.Enable();
        }

        public void DisableBattleInput()
        {
            DisableAllInput();
            EnableMapGameplayInput();
        }

        public void EnableBattleInput()
        {
            DisableAllInput();
            InputActions.BattleMenu.Enable();
        }

        #endregion

        #region BattleMenuActions

        public void OnNavigate(InputAction.CallbackContext context)
        {
            NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) SubmitEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.performed) ConfirmedEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }

        #endregion
    }
}