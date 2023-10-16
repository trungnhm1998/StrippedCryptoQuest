namespace CryptoQuest.States
{
    public class TransitionProgressingState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public TransitionProgressingState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            _presenter.TransitionProgressed();
        }

        public void OnExit()
        {
        }
    }
}