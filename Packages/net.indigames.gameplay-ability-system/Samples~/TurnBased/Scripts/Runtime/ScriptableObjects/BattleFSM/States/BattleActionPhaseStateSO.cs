using UnityEngine;
using System.Collections;
using Indigames.AbilitySystem.FSM;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleActionPhaseStateSO", menuName = "Indigames Ability System/FSM/States/Battle Action Phase State")]
    public class BattleActionPhaseStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            stateMachine.StartCoroutine(BattleUnitsAction(stateMachine));
            Debug.Log($"Battle Enter Action Phase");
        }
        
        private IEnumerator BattleUnitsAction(BaseStateMachine stateMachine)
        {
            foreach (var unit in _battleManager.BattleUnits)
            {
                yield return unit.Execute();
            }
            stateMachine.SetCurrentState(NextState);
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleUnitsAction(stateMachine));
            Debug.Log($"Battle Exit Action Phase");
        }
    }
}