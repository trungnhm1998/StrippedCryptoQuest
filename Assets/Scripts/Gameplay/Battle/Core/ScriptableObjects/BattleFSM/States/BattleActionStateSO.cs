using System.Collections;
using CryptoQuest.FSM;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleActionStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Action State")]
    public class BattleActionStateSO : BattleStateSO
    {        
        [SerializeField] private VoidEventChannelSO _endActionPhaseEventChannel;

        private Coroutine _unitActionCoroutine; 

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _unitActionCoroutine = stateMachine.StartCoroutine(BattleUnitsAction(stateMachine));
        }
        
        private IEnumerator BattleUnitsAction(BaseStateMachine stateMachine)
        {
            foreach (var unit in BattleManager.BattleUnits)
            {
                BattleManager.CurrentUnit = unit;
                yield return unit.Execute();
            }
            BattleManager.CurrentUnit = NullBattleUnit.Instance;
            _endActionPhaseEventChannel.RaiseEvent();
            stateMachine.SetCurrentState(_nextState);
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(_unitActionCoroutine);
        }
    }
}