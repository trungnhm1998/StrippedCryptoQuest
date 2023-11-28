using FSM;

namespace CryptoQuest.BlackSmith.State.Overview
{
    public class OverviewState : BlackSmithStateBase
    {
        private readonly UIBlackSmithOverview _ui;
        private readonly BlackSmithDialogsPresenter _dialogPresenter;

        public OverviewState(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _ui = _manager.OverviewUI;
            _dialogPresenter = _manager.DialogPresenter;
            _manager.BlackSmithInput.EnableInput();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _ui.OnUpgradeButtonPressed += ChangeToUpgradeState;
            _ui.OnEvolveButtonPressed += ChangeToEvolveState;

            _ui.Show();
            _dialogPresenter.ShowOverviewDialog();
        }

        public override void OnExit()
        {
            base.OnExit();
            RemoveEvents();

            _ui.Hide();
            _dialogPresenter.HideOverviewDialog();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _ui.OnUpgradeButtonPressed -= ChangeToUpgradeState;
            _ui.OnEvolveButtonPressed -= ChangeToEvolveState;
        }

        protected override void OnCancel()
        {
            OnExit();
            _manager.BlackSmithInput.DisableInput();
        }

        private void ChangeToUpgradeState()
        {
            fsm.RequestStateChange(Contants.UPGRADE_STATE);
        }

        private void ChangeToEvolveState()
        {
            fsm.RequestStateChange(Contants.EVOLVE_STATE);
        }
    }
}