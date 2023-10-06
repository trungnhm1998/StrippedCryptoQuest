using CryptoQuest.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    public class BattleInput : ScriptableSingleton<BattleInput>,
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
            InputActions.BattleMenu.RemoveCallbacks(this);
        }

        public void EnableBattleInput()
        {
            DisableAllInput();
            InputActions.BattleMenu.Enable();
            InputActions.BattleMenu.SetCallbacks(this);
        }

        #endregion

        #region BattleMenuActions

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) NavigateEvent?.Invoke(context.ReadValue<Vector2>());
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