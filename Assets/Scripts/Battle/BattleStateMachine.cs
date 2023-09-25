using CryptoQuest.Input;
using CryptoQuest.PushdownFSM;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattleStateMachine : MonoBehaviour
    {
        [SerializeField] private BattleInputSO _battleInput;

        private readonly PushDownStateMachine _fsm = new();

        private void OnEnable()
        {
            _battleInput.CancelEvent += GoToPreviousState;
        }

        private void OnDisable()
        {
            _battleInput.CancelEvent -= GoToPreviousState;
        }

        public void GoToPreviousState() => _fsm.Pop();

        public void GoToState(IState state)
        {
            Debug.Log($"BattleStateMachine: GoToState {state.GetType().Name}");
            _fsm.Push(state);
        }
    }
}