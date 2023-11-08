namespace CryptoQuest.Menus.DimensionalBox.States
{
    internal abstract class StateBase
    {
        protected DimensionalBoxStateMachine StateMachine { get; private set; }

        public void Enter(DimensionalBoxStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            OnEnter();
        }

        protected virtual void OnEnter() { }

        public void Exit()
        {
            OnExit();
        }

        protected virtual void OnExit() { }
    }
}