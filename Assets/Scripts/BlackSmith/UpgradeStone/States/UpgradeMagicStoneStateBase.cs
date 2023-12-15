using CryptoQuest.BlackSmith.UpgradeStone.UI;
using FSM;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class UpgradeMagicStoneStateBase : StateBase<EUpgradeMagicStoneStates>
    {
        protected UpgradeMagicStoneStateMachine _stateMachine { get; }
        protected UpgradableStonesPresenter _upgradableStonePresenter => _stateMachine.UpgradableStonesPresenter;
        protected MaterialStonesPresenter _materialStonesPresenter => _stateMachine.MaterialStonesPresenter;
        protected BlackSmithDialogsPresenter _dialogsPresenter => _stateMachine.DialogsPresenter;
        protected UIUpgradeMagicStoneToolTip _magicStoneTooltip => _stateMachine.MagicStoneTooltip;
        protected StoneListPresenter _listPresenter => _stateMachine.ListPresenter;
        protected StoneUpgradePresenter _stoneUpgradePresenter => _stateMachine.StoneUpgradePresenter;

        protected CurrencyPresenter _currencyPresenter => _stateMachine.CurrencyPresenter;
        protected UpgradeStoneResultPresenter _upgradeStoneResultPresenter =>
            _stateMachine.UpgradeStoneResultPresenter;

        protected ConfirmStoneUpgradePresenter _confirmUpgradePresenter =>
            _stateMachine.ConfirmUpgradePresenter;

        protected UpgradeMagicStoneStateBase(UpgradeMagicStoneStateMachine stateMachine) : base(false) =>
            _stateMachine = stateMachine;

        public override void OnEnter() => _stateMachine.SetCurrentState(this);

        public override void OnExit() => _stateMachine.SetCurrentState(null);
        public virtual void OnCancel() { }
        public virtual void OnSubmit() { }
    }
}