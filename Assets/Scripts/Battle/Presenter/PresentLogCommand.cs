using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Presenter
{
    public class PresentLogCommand : IPresentCommand
    {
        private readonly LocalizedString _message;
        public PresentLogCommand(LocalizedString message) => _message = message;
        public StateBase GetState() => new PresentLogState(_message);
    }

    public class PresentLogState : StateBase
    {
        private readonly LocalizedString _message;
        private bool _waited;
        private IEnumerator _coWait;

        public PresentLogState(LocalizedString message) => _message = message;

        protected override void OnEnter()
        {
            // Listen to enter pressed to stop coroutine and change to next state
            StartCoroutine(CoPresentLog());
        }

        protected override void OnExit()
        {
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StopCoroutine(CoPresentLog());
        }

        private void StopWaiting()
        {
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StopCoroutine(_coWait);
            _waited = true;
        }

        private IEnumerator CoPresentLog()
        {
            _waited = false;
            var handle = _message.GetLocalizedStringAsync();
            yield return handle;
            StateMachine.Input.ConfirmedEvent += StopWaiting;
            _coWait = CoAppendAndWait(handle.Result);
            StartCoroutine(_coWait);
            yield return new WaitUntil(() => _waited);
            StateMachine.Input.ConfirmedEvent -= StopWaiting;
            StateMachine.ChangeState(StateMachine.GetNextCommand);
        }

        private IEnumerator CoAppendAndWait(string message)
        {
            LogPresenter.Append(message);
            yield return new WaitForSeconds(LogPresenter.DelayBetweenLines);
            _waited = true;
        }
    }
}