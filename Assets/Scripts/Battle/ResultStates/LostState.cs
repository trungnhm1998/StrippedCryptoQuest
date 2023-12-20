using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.ResultStates
{
    public class LostState : MonoBehaviour, IResultState
    {
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleBus _battleBus;

        public EBattleResult Result => EBattleResult.Lost;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            stateMachine.StartCoroutine(CoOnPresentLost());
        }

        public void OnExit(BattleStateMachine stateMachine) { }


        public void RaiseSetEvent()
        {
            BattleEventBus.RaiseEvent(new TurnLostEvent());
        }

        private IEnumerator CoOnPresentLost()
        {
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleLostEvent { Battlefield = _battleBus.CurrentBattlefield });
        }
    }
}