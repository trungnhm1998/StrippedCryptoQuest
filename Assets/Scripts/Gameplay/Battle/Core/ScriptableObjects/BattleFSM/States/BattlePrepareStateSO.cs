using UnityEngine;
using System.Collections;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattlePrepareStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Prepare State")]
    public class BattlePrepareStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            Debug.Log($"BattleState: Enter Prepare Phase");
            stateMachine.StartCoroutine(BattlePrepare(stateMachine));
        }

        private IEnumerator BattlePrepare(BaseStateMachine stateMachine)
        {
            if (_battleManager.BattleSpawner)
            {
                _battleManager.BattleSpawner.SpawnBattle();
            }
            _battleManager.InitBattleUnits();
            stateMachine.SetCurrentState(NextState);
            yield break;
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattlePrepare(stateMachine));
            Debug.Log($"BattleState: Exit Prepare Phase");
        }
    }
}