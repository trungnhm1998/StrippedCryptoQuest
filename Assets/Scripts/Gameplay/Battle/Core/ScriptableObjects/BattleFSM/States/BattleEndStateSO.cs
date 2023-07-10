using UnityEngine;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleEndStateSO", menuName = "Gameplay/Battle/FSM/States/Battle End State")]
    public class BattleEndStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            BattleEnd(stateMachine);
        }

        private void BattleEnd(BaseStateMachine stateMachine)
        {
            // Unload battle scene here
        }
    }
}