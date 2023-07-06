using UnityEngine;
using System.Collections;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleStrategyStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Strategy State")]
    public class BattleStrategyStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            Debug.Log($"BattleState: Enter Strategy Phase");
            stateMachine.StartCoroutine(BattleUnitsPrepare(stateMachine));
        }

        private IEnumerator BattleUnitsPrepare(BaseStateMachine stateMachine)
        {
            // Might sort _battleUnits by speed or team 1 will execute first by default
            _battleManager.OnNewTurn();
            foreach (var unit in _battleManager.BattleUnits)
            {
                _battleManager.CurrentUnit = unit;
                yield return unit.Prepare();
            }
            _battleManager.CurrentUnit = null;
            stateMachine.SetCurrentState(NextState);
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleUnitsPrepare(stateMachine));
            Debug.Log($"BattleState: Exit Strategy Phase");
        }
    }
}