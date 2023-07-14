using UnityEngine;
using System.Collections;
using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
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
                yield return unit.Execute();
            }
            _endActionPhaseEventChannel.RaiseEvent();
        }

        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(_unitActionCoroutine);
        }
    }
}