using CryptoQuest.States;
using UnityEngine;

namespace CryptoQuest
{
    public interface ITransitionState
    {
        public void OnEnter();
        public void OnExit();
    }

    public class TransitionSystem : MonoBehaviour
    {
        [SerializeField] private TransitionEventChannelSO _requestTransition;
        [field: SerializeField] public TransitionPresenter Presenter { get; private set; }
        private ITransitionState _curState;
        public ITransitionState CurState => _curState;

        private void OnEnable()
        {
            _requestTransition.EventRaised += HandleTransitionRequest;
            ChangeState(new TransitionIdleState(this));
        }

        private void OnDisable()
        {
            _requestTransition.EventRaised -= HandleTransitionRequest;
        }

        private void HandleTransitionRequest(AbstractTransition transition)
        {
            ChangeState(transition.GetTransitionState(this));
        }

        public void ChangeState(ITransitionState state)
        {
            Debug.Log("Transition state changed to " + state);
            if (_curState != null)
                _curState.OnExit();

            _curState = state;
            _curState.OnEnter();
        }
    }
}