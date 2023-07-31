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
            _unitActionCoroutine = stateMachine.StartCoroutine(PerformBattleUnitsAction(stateMachine));
        }

        private IEnumerator PerformBattleUnitsAction(BaseStateMachine stateMachine)
        {
            foreach (var unit in BattleManager.GetActionOrderList())
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
            if (_unitActionCoroutine == null) return;
            stateMachine.StopCoroutine(_unitActionCoroutine);
        }
    }
}