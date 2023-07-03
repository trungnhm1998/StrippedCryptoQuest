using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.FSM;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleStrategyPhaseStateSO", menuName = "Indigames Ability System/FSM/States/Battle Strategy Phase State")]
    public class BattleStrategyPhaseStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            Debug.Log($"Battle Enter Strategy Phase");
            stateMachine.StartCoroutine(BattleUnitsPrepare(stateMachine));
        }

        private IEnumerator BattleUnitsPrepare(BaseStateMachine stateMachine)
        {
            foreach (var unit in _battleManager.BattleUnits)
            {
                yield return unit.Prepare();
            }
            stateMachine.SetCurrentState(NextState);
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleUnitsPrepare(stateMachine));
            Debug.Log($"Battle Exit Strategy Phase");
        }
    }
}