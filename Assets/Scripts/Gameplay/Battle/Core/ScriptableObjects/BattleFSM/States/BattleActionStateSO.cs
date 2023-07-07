using UnityEngine;
using System.Collections;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleActionStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Action State")]
    public class BattleActionStateSO : BattleStateSO
    {
        private BattleLog _battleLog;

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _battleLog = stateMachine.GetComponent<BattleLog>();
            stateMachine.StartCoroutine(BattleUnitsAction(stateMachine));
            Debug.Log($"BattleState: Enter Action Phase");
        }
        
        private IEnumerator BattleUnitsAction(BaseStateMachine stateMachine)
        {
            foreach (var unit in _battleManager.BattleUnits)
            {
                yield return unit.Execute();

                if (!_battleLog) continue;
                foreach (var log in unit.ExecuteLogs)
                {
                    _battleLog.Log(new BattleLogData() {
                        Message = log
                    });
                    yield return null;
                }
                unit.ExecuteLogs.Clear();
            }
            stateMachine.SetCurrentState(NextState);
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleUnitsAction(stateMachine));
            Debug.Log($"BattleState: Exit Action Phase");
        }
    }
}