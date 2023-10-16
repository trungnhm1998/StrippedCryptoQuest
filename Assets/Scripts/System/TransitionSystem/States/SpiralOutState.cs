namespace CryptoQuest.States
{
    public class SpiralOutState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public SpiralOutState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            _presenter.SpiralOut();
            _presenter.OnTransitionOutComplete += HandleFadeOutComplete;
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