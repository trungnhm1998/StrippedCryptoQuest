using CryptoQuest.BlackSmith.UpgradeStone.UI;
using FSM;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class UpgradeMagicStoneStateBase : StateBase<EUpgradeMagicStoneStates>
    {
        protected UpgradeMagicStoneStateMachine _stateMachine { get; }
        protected UIUpgradableStoneList _upgradableStoneListUI => _stateMachine.UpgradableStoneListUI;
        protected UIMaterialStoneList _materialStoneList => _stateMachine.MaterialStoneListUI;
        protected BlackSmithDialogsPresenter _dialogsPresenter => _stateMachine.DialogsPresenter;
        protected UIUpgradeMagicStoneToolTip _magicStoneTooltip => _stateMachine.MagicStoneTooltip;
        protected StoneListPresenter _listPresenter => _stateMachine.ListPresenter;
        protected StoneUpgradePresenter _stoneUpgradePresenter => _stateMachine.StoneUpgradePresenter;

        protected UpgradeStoneResultPresenter _upgradeStoneResultPresenter =>
            _stateMachine.UpgradeStoneResultPresenter;

        protected ConfirmStoneUpgradePresenter _confirmUpgradePresenter =>
            _stateMachine.ConfirmUpgradePresenter;

        protected UpgradeMagicStoneStateBase(UpgradeMagicStoneStateMachine stateMachine) : base(false) =>
            _stateMachine = stateMachine;

        public override void OnEnter() => _stateMachine.SetCurrentState(this);

        public override void OnExit() => _stateMachine.SetCurrentState(null);
        public virtual void OnCancel() { }
    }
}