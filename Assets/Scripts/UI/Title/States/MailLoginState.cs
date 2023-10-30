﻿using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using TinyMessenger;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public class MailLoginState : InputStateBase
    {
        private UISignInPanel _uiSignIn;
        private TinyMessageSubscriptionToken _authorizedToken;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            stateMachine.TryGetComponentInChildren(out _uiSignIn);
            _uiSignIn.gameObject.SetActive(true);

            _authorizedToken = ActionDispatcher.Bind<AuthenticateSucceed>(Authorized);
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            _uiSignIn.gameObject.SetActive(false);
            
            ActionDispatcher.Unbind(_authorizedToken);
        }
        
        private void Authorized(AuthenticateSucceed _)
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