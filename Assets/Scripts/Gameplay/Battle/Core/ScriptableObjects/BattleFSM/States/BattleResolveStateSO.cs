using System.Collections;
using CryptoQuest.FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.States
{
    [CreateAssetMenu(fileName = "BattleResolveStateSO", menuName = "Gameplay/Battle/FSM/States/Battle Resolve State")]
    public class BattleResolveStateSO : BattleStateSO
    {
        [SerializeField] private VoidEventChannelSO _turnEndChannelEvent;
        private bool _isResolveComplete;
        private Coroutine _unitResolveCoroutine; 

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            base.OnEnterState(stateMachine);
            _isResolveComplete = false;
            _unitResolveCoroutine = stateMachine.StartCoroutine(BattleUnitsResolve());
        }

        private IEnumerator BattleUnitsResolve()
        {
            yield return BattleManager.RemovePendingUnits();

            _turnEndChannelEvent.RaiseEvent();

            foreach (var unit in BattleManager.BattleUnits)
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
            stateMachine.StopCoroutine(_unitResolveCoroutine);
        }
    }
}