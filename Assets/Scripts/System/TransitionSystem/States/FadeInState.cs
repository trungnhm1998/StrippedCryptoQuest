using CryptoQuest.System.TransitionSystem;
using CryptoQuest.System.TransitionSystem.States;

namespace CryptoQuest.States
{
    public class FadeInState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public FadeInState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            _presenter.OnTransitionInComplete += HandleFadeInComplete;
            _presenter.Fadein();
        }

        private void HandleFadeInComplete()
        {
            _transitionSystem.ChangeState(new TransitionProgressingState(_transitionSystem));
            _presenter.OnTransitionInComplete -= HandleFadeInComplete;
        }

        public void OnExit()
        {
            _presenter.ResetToDefault();
        }
    }
}