using CryptoQuest.FSM;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleEndStateSO", menuName = "Gameplay/Battle/FSM/States/Battle End State")]
    public class BattleEndStateSO : BattleStateSO
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private SceneScriptableObject _battleSceneSO;
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            BattleEnd(stateMachine);
        }

        private void BattleEnd(BaseStateMachine stateMachine)
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
            _gameState.UpdateGameState(EGameState.Field);
        }
    }
}