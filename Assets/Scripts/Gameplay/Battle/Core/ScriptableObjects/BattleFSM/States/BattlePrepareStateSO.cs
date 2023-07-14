using UnityEngine;
using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattlePrepareStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Prepare State")]
    public class BattlePrepareStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _battleStartChannelEvent;

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
            _battleStartChannelEvent.RaiseEvent();
            stateMachine.SetCurrentState(_nextState);
        }
    }
}