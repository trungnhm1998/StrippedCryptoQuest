using CryptoQuest.Input;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public class InputStateBase : IState, InputActions.ITitleActions
    {
        private InputMediatorSO _inputMediator;
        protected TitleStateMachine StateMachine { get; private set; }

        public virtual void OnEnter(TitleStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            _inputMediator = ServiceProvider.GetService<InputMediatorSO>();
            _inputMediator.InputActions.Title.SetCallbacks(this);
            _inputMediator.EnableInputMap("Title");
        }

        public virtual void OnExit(TitleStateMachine stateMachine)
        {
            _inputMediator.InputActions.Title.RemoveCallbacks(this);
            _inputMediator.DisableInputMap("Title");
        }

        public virtual void OnNavigate(InputAction.CallbackContext context) { }

        public virtual void OnClick(InputAction.CallbackContext context) { }

        public virtual void OnCancel(InputAction.CallbackContext context) { }

        public virtual void OnConfirm(InputAction.CallbackContext context) { }

        public virtual void OnTab(InputAction.CallbackContext context) { }

        protected bool IsShiftHolding { get; private set; }

        public void OnShift(InputAction.CallbackContext context)
        {
            IsShiftHolding = context.performed;
        }
    }
}