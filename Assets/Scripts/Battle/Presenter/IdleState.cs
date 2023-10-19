using System.Collections;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter
{
    public class IdleState : StateBase
    {
        protected override void OnEnter()
        {
            LogPresenter.Show();
            StartCoroutine(CoWaitUntilCommandEnqueue());
        }

        protected override void OnExit()
        {
            StopCoroutine(CoWaitUntilCommandEnqueue());
        }

        private IEnumerator CoWaitUntilCommandEnqueue()
        {
            yield return new WaitUntil(() => StateMachine.Commands.Count > 0);
            StateMachine.ChangeState(StateMachine.Commands.Dequeue().GetState());
        }
    }
}