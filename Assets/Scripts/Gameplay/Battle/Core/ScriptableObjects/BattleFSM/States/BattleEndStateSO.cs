using UnityEngine;
using System.Collections;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleEndStateSO", menuName = "Gameplay/Battle/FSM/States/Battle End State")]
    public class BattleEndStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            Debug.Log($"BattleState: Enter End Phase");
            stateMachine.StartCoroutine(BattleEnd(stateMachine));
        }

        private IEnumerator BattleEnd(BaseStateMachine stateMachine)
        {
            // Unload battle scene here
            yield break;
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleEnd(stateMachine));
            Debug.Log($"BattleState: Exit End Phase");
        }
    }
}