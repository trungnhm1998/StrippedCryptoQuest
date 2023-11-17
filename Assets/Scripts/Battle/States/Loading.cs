using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;

namespace CryptoQuest.Battle.States
{
    public class Loading : IState
    {
        private IBattleInitializer _initializer;
        private BattleStateMachine _battleStateMachine;
        private BattleTransitionPresenter _transitionPresenter;

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            battleStateMachine.TryGetPresenterComponent<BattleTransitionPresenter>(out _transitionPresenter);
            _initializer = battleStateMachine.GetComponent<IBattleInitializer>();
            InitBattle();
        }

        public void OnExit(BattleStateMachine battleStateMachine)
        {
        }

        private void InitBattle() => _battleStateMachine.StartCoroutine(CoInitBattle());

        private IEnumerator CoInitBattle()
        {
            yield return _initializer.LoadEnemies();
            yield return GenericDialogController.Instance.CoInstantiate(ChangeToIntroState);
            yield return new WaitForSeconds(_transitionPresenter.TransitDuration);
            _transitionPresenter.TransitOut();
        }

        private void ChangeToIntroState(UIGenericDialog dialog)
        {
            _battleStateMachine.ChangeState(new Intro(dialog));
            BattleEventBus.RaiseEvent(new LoadedEvent());
        }
    }
}