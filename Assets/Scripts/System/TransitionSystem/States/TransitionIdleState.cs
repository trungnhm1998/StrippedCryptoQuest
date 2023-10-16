namespace CryptoQuest.States
{
    public class TransitionIdleState : ITransitionState
    {
        private TransitionSystem _transitionSystem;
        private TransitionPresenter _presenter;

        public TransitionIdleState(TransitionSystem transitionSystem)
        {
            _transitionSystem = transitionSystem;
            _presenter = transitionSystem.Presenter;
        }

        public void OnEnter()
        {
            _presenter.ResetToDefault();
        }

        public void OnExit()
        {
        }
    }
}