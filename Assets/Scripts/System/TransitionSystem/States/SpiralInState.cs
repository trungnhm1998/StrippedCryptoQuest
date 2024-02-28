namespace CryptoQuest.System.TransitionSystem.States
{
    public class SpiralInState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public SpiralInState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            if (_transitionSystem.CurState is TransitionProgressingState) return;
            _presenter.SpiralIn();
            _presenter.OnTransitionInComplete += HandleFadeInComplete;
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