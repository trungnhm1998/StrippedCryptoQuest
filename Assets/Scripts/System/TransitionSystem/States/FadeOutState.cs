using CryptoQuest.States;

namespace CryptoQuest.System.TransitionSystem.States
{
    public class FadeOutState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public FadeOutState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            _presenter.ResetToDefault();
            _presenter.OnTransitionOutComplete += HandleFadeOutComplete;
            _presenter.FadeOut();
        }

        private void HandleFadeOutComplete()
        {
            _transitionSystem.ChangeState(new TransitionIdleState(_transitionSystem));
            _presenter.OnTransitionOutComplete -= HandleFadeOutComplete;
        }

        public void OnExit()
        {
            _presenter.ResetToDefault();
        }
    }
}