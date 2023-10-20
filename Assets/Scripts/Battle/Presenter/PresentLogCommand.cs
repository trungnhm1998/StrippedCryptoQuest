using System.Collections;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter
{
    public class PresentLogCommand : IPresentCommand
    {
        private readonly string _message;
        public PresentLogCommand(string message) => _message = message;
        public StateBase GetState() => new PresentLogState(_message);
    }

    public class PresentLogState : StateBase
    {
        private readonly string _message;
        private bool _waited;

        public PresentLogState(string message) => _message = message;

        protected override void OnEnter()
        {
            // Listen to enter pressed to stop coroutine and change to next state
            StartCoroutine(CoPresentLog());
            StateMachine.Input.ConfirmedEvent += StopWaiting;
        }

        protected override void OnExit()
        {
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StopCoroutine(CoPresentLog());
        }

        private void StopWaiting()
        {
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StopCoroutine(CoAppendAndWait());
            _waited = true;
        }

        private IEnumerator CoPresentLog()
        {
            _waited = false;
            StartCoroutine(CoAppendAndWait());
            yield return new WaitUntil(() => _waited);
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StateMachine.ChangeState(StateMachine.GetNextCommand);
        }

        private IEnumerator CoAppendAndWait()
        {
            LogPresenter.Append(_message);
            yield return new WaitForSeconds(LogPresenter.DelayBetweenLines);
            _waited = true;
        }
    }
}