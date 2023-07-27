using System.Collections;
using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleStrategyStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Strategy State")]
    public class BattleStrategyStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _endStrategyPhaseEventChannel;
        private Coroutine _unitPrepareCoroutine; 
        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _unitPrepareCoroutine = stateMachine.StartCoroutine(BattleUnitsPrepare(stateMachine));
        }

        private IEnumerator BattleUnitsPrepare(BaseStateMachine stateMachine)
        {
            // Might sort _battleUnits by speed or team 1 will execute first by default
            BattleManager.OnNewTurn();
            foreach (var unit in BattleManager.BattleUnits)
            {
                BattleManager.CurrentUnit = unit;
                yield return unit.Prepare();
            }
            BattleManager.CurrentUnit = NullBattleUnit.Instance;
            stateMachine.SetCurrentState(_nextState);
            _endStrategyPhaseEventChannel.RaiseEvent();
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(_unitPrepareCoroutine);
        }
    }
}