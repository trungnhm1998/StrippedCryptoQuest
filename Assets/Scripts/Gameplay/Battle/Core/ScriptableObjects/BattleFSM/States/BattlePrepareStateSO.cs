using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattlePrepareStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Prepare State")]
    public class BattlePrepareStateSO : BattleStateSO
    {
        [SerializeField] private GameStateSO _gameState;

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
            _gameState.UpdateGameState(EGameState.Battle);
            stateMachine.SetCurrentState(_nextState);
        }
    }
}