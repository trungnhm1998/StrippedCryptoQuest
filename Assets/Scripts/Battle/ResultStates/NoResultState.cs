using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.States.SelectHeroesActions;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.ResultStates
{
    /// <summary>
    /// Change <see cref="BattleStateMachine"/> to the next state after all the actions has been presented
    /// </summary>
    public class NoResultState : MonoBehaviour, IResultState
    {
        public EBattleResult Result => EBattleResult.None;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            stateMachine.ChangeState(new SelectHeroesActions());
        }

        public void OnExit(BattleStateMachine stateMachine) { }
        public void RaiseSetEvent() { }
    }
}