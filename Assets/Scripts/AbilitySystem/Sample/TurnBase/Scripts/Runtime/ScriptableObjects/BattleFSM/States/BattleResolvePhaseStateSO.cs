using UnityEngine;
using System.Collections;
using Indigames.AbilitySystem.FSM;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleResolvePhaseStateSO", menuName = "Indigames Ability System/FSM/States/Battle Resolve Phase State")]
    public class BattleResolvePhaseStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _turnEndChannelEvent;
        private bool _isResolveComplete;

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _isResolveComplete = false;
            Debug.Log($"Battle Enter Resolve Phase");
            stateMachine.StartCoroutine(BattleUnitsResolve());
        }

        private IEnumerator BattleUnitsResolve()
        {
            yield return _battleManager.RemovePendingUnits();

            _turnEndChannelEvent.RaiseEvent();

            foreach (var unit in _battleManager.BattleUnits)
            {
                yield return unit.Resolve();
            }
            _isResolveComplete = true;
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (!_isResolveComplete) return;
            base.Execute(stateMachine);
        }
        
        public override void OnExitState(BaseStateMachine stateMachine)
        {
            base.OnExitState(stateMachine);
            stateMachine.StopCoroutine(BattleUnitsResolve());
            Debug.Log($"Battle Exit Resolve Phase");
        }
    }
}