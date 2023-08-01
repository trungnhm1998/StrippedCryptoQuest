using CryptoQuest.FSM;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleEndStateSO", menuName = "Gameplay/Battle/FSM/States/Battle End State")]
    public class BattleEndStateSO : BattleStateSO
    {
        [SerializeField] private GameStateSO _gameState;
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            BattleEnd(stateMachine);
        }

        private void BattleEnd(BaseStateMachine stateMachine)
        {
            BattleManager.OnBattleEnd();
            _gameState.UpdateGameState(EGameState.Field);
        }
    }
}