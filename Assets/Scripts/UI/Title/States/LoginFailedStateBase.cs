using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.UI.Title.States
{
    public abstract class LoginFailedStateBase : InputStateBase
    {
        private Coroutine _autoCloseCo;

        public override void OnEnter(TitleStateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            stateMachine.LoginFailedPanel.SetActive(true);
            _autoCloseCo = stateMachine.StartCoroutine(WaitAndChangeStateCo());
        }

        public override void OnExit(TitleStateMachine stateMachine)
        {
            base.OnExit(stateMachine);
            stateMachine.LoginFailedPanel.SetActive(false);
        }

        private IEnumerator WaitAndChangeStateCo()
        {
            yield return new WaitForSeconds(StateMachine.AutoCloseLoginFailedPanelTime);
            ChangeState();
        }

        public override void OnConfirm(InputAction.CallbackContext context)
            => ChangeState();

        public override void OnCancel(InputAction.CallbackContext context)
            => ChangeState();

        private void ChangeState()
        {
            if (_autoCloseCo != null)
                StateMachine.StopCoroutine(_autoCloseCo);
            StateMachine.ChangeState(GetState());
        }

        protected abstract IState GetState();
    }
}