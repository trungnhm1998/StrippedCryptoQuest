using CryptoQuest.BlackSmith.Upgrade;
using FSM;

namespace CryptoQuest.BlackSmith.State
{
    public class BlackSmithStateBase : StateBase
    {
        protected readonly BlackSmithStateMachine _blackSmithFSM;
        protected readonly BlackSmithManager _manager;
        protected readonly UpgradePresenter _upgradePresenter;

        public BlackSmithStateBase(BlackSmithStateMachine stateMachine) : base(false)
        {
            _blackSmithFSM = stateMachine;
            _manager = _blackSmithFSM.BlackSmithManager;
            _upgradePresenter = _manager.UpgradePresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _blackSmithFSM.CurrentState = this;
            _manager.BlackSmithInput.CancelEvent += OnCancel;
        }

        public override void OnExit()
        {
            base.OnExit();
            RemoveEvents();
        }

        protected virtual void RemoveEvents()
        {
            _manager.BlackSmithInput.CancelEvent -= OnCancel;
        }

        protected virtual void OnCancel() { }
        public virtual void OnDestroy()
        {
            RemoveEvents();
        }
    }
}