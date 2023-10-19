using System.Collections;
using CryptoQuest.Battle.UI.Logs;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter
{
    public abstract class StateBase
    {
        protected VfxAndLogPresenter StateMachine { get; private set; }
        protected LogPresenter LogPresenter { get; private set; }

        public void Enter(VfxAndLogPresenter stateMachine, LogPresenter logPresenter)
        {
            StateMachine = stateMachine;
            LogPresenter = logPresenter;
            OnEnter();
        }

        protected virtual void OnEnter() { }

        public void Exit() => OnExit();

        protected virtual void OnExit() { }

        public void Update() => OnUpdate();

        protected virtual void OnUpdate() { }

        protected Coroutine StartCoroutine(IEnumerator coroutine) => StateMachine.StartCoroutine(coroutine);
        protected void StopCoroutine(IEnumerator coroutine) => StateMachine.StopCoroutine(coroutine);
    }
}