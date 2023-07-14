using UnityEngine;
using CryptoQuest.FSM;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleEndStateSO", menuName = "Gameplay/Battle/FSM/States/Battle End State")]
    public class BattleEndStateSO : BattleStateSO
    {
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
        }
    }
}