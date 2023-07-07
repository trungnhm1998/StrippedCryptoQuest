using UnityEngine;
using System.Collections.Generic;

namespace CryptoQuest.FSM
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
                break;
            }
        }
    }
}