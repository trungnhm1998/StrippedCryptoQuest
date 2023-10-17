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

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
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
            yield return new WaitForSeconds(_battleStateMachine.Spiral.Duration);
            var transition = _battleStateMachine.TransitionOut;
            _battleStateMachine.TransitionEventChannelSo.RaiseEvent(transition);
        }

        private void ChangeToIntroState(UIGenericDialog dialog)
        {
            _battleStateMachine.ChangeState(new Intro(dialog));
            BattleEventBus.RaiseEvent(new LoadedEvent());
        }
    }
}