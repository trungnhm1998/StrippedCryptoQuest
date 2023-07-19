using System.Collections.Generic;
using CryptoQuest.FSM.ScriptableObjects.Base;
using UnityEngine;

namespace CryptoQuest.FSM.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "Gameplay/Battle/FSM/States/Normal State")]
    public class StateSO : BaseStateSO
    {
        [SerializeField] private List<FSMActionSO> _actions;
        [SerializeField] private List<TransitionSO> _transitions;
        [SerializeField] protected BaseStateSO _nextState;

        public override void Execute(BaseStateMachine stateMachine)
        {
            foreach (FSMActionSO action in _actions)
            {
                action.Execute(stateMachine);
            }

            foreach (TransitionSO transition in _transitions)
            {
                transition.Execute(stateMachine);
                if (stateMachine.CurrentState != this) break;
            }
        }

        public override void OnEnterState(BaseStateMachine stateMachine) {}
        
        public override void OnExitState(BaseStateMachine stateMachine) {}
    }
}