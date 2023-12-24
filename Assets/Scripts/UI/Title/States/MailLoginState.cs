using System;
using CryptoQuest.Actions;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    [Obsolete]
    public class MailLoginState : InputStateBase
    {
        private UISignInPanel _uiSignIn;
        private TinyMessageSubscriptionToken _getProfileSucceed;
        private TinyMessageSubscriptionToken _authFailedEvent;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            stateMachine.TryGetComponentInChildren(out _uiSignIn);
            _uiSignIn.gameObject.SetActive(true);

            _getProfileSucceed = ActionDispatcher.Bind<GetProfileSucceed>(Authorized);
            _authFailedEvent = ActionDispatcher.Bind<AuthenticateFailed>(_ => stateMachine.ChangeState(new MailLoginFailed()));
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            _uiSignIn.gameObject.SetActive(false);
            
            ActionDispatcher.Unbind(_authFailedEvent);
            ActionDispatcher.Unbind(_getProfileSucceed);
        }
        
        private void Authorized(GetProfileSucceed _)
        {
            StateMachine.ChangeState(new StartGameState());
        }

        public override void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) _uiSignIn.HandleDirection(context.ReadValue<Vector2>().y);
        }

        public override void OnTab(InputAction.CallbackContext context)
        {
            if (context.performed == false) return;
            var reverse = IsShiftHolding ? -1 : 1;
            _uiSignIn.HandleDirection(-1f * reverse);
        }

        public override void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) StateMachine.ChangeState(new TitleState());
        }
    }
}