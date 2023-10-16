using CryptoQuest.States;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem
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

        [Header("Transition for Scene Loaded")]
        [SerializeField] private AbstractTransition _sceneLoadedTransition;

        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannelSO;

        private ITransitionState _curState;
        public ITransitionState CurState => _curState;

        private void OnEnable()
        {
            _requestTransition.EventRaised += HandleTransitionRequest;
            _onSceneLoadedEventChannelSO.EventRaised += HandleSceneLoaded;
            ChangeState(new TransitionIdleState(this));
        }

        private void OnDisable()
        {
            _requestTransition.EventRaised -= HandleTransitionRequest;
            _onSceneLoadedEventChannelSO.EventRaised -= HandleSceneLoaded;
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

        private void HandleSceneLoaded()
        {
            HandleTransitionRequest(_sceneLoadedTransition);
        }
    }
}