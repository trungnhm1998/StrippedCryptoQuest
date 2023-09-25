using System.Collections.Generic;

namespace CryptoQuest.PushdownFSM
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }

    public class PushDownStateMachine
    {
        private readonly Stack<IState> _stack = new();
        private IState _currentState;

        public void Push(IState state)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _stack.Push(state);
            _currentState = state;
            _currentState.OnEnter();
        }

        public void Pop()
        {
            _currentState?.OnExit();
            _stack.Pop();
            if (_stack.Count == 0) return;
            _currentState = _stack.Peek();
            _currentState?.OnEnter();
        }
    }
}