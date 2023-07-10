using UnityEngine;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattlePrepareStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Prepare State")]
    public class BattlePrepareStateSO : BattleStateSO
    {
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            BattlePrepare(stateMachine);
        }

        private void BattlePrepare(BaseStateMachine stateMachine)
        {
            if (BattleManager.BattleSpawner)
            {
                BattleManager.BattleSpawner.SpawnBattle();
            }
            BattleManager.InitBattleTeams();
            stateMachine.SetCurrentState(_nextState);
        }
    }
}