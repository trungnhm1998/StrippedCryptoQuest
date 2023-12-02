using FSM;

namespace CryptoQuest.BlackSmith.States
{
    public abstract class BlackSmithStateBase : StateBase
    {
        protected BlackSmithSystem Context { get; }

        protected BlackSmithStateBase(BlackSmithSystem context) : base(false) => Context = context;

        public override void OnEnter() => Context.Input.CancelEvent += OnCancel;

        public override void OnExit() => Context.Input.CancelEvent -= OnCancel;

        protected virtual void OnCancel() { }
    }
}