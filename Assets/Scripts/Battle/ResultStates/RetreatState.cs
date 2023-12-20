using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.ResultStates
{
    public class RetreatState : MonoBehaviour, IResultState
    {
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleBus _bus;

        public EBattleResult Result => EBattleResult.Retreated;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            stateMachine.StartCoroutine(CoOnPresentEnd());
        }

        public void OnExit(BattleStateMachine stateMachine) { }

        public void RaiseSetEvent()
        {
            BattleEventBus.RaiseEvent(new RetreatedEvent());
        }

        private IEnumerator CoOnPresentEnd()
        {
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleRetreatedEvent { Battlefield = _bus.CurrentBattlefield, });
        }
    }
}