using UnityEngine;


namespace CryptoQuest.UI.Title
{
    public interface IState
    {
        public void OnEnter();
        public void OnExit();
    }

    public class TitleStateMachine : MonoBehaviour
    {
        private IState _curState;

        public void ChangeState(IState state)
        {
            if (_curState != null)
                _curState.OnExit();

            _curState = state;
            _curState.OnEnter();
        }
    }
}