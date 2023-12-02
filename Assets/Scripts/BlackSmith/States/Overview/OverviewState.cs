namespace CryptoQuest.BlackSmith.States.Overview
{
    public class OverviewState : BlackSmithStateBase
    {
        private readonly UIBlackSmithOverview _ui;
        private readonly BlackSmithDialogsPresenter _dialogPresenter;

        public OverviewState(BlackSmithSystem context) : base(context)
        {
            _ui = context.OverviewUI;
            _dialogPresenter = context.DialogPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Context.Input.EnableInput();

            _ui.OpenUpgrading += ChangeToUpgradeState;
            _ui.OpenEvolving += ChangeToEvolveState;

            _ui.gameObject.SetActive(true);
            _dialogPresenter.ShowOverviewDialog();
        }

        public override void OnExit()
        {
            base.OnExit();

            _ui.gameObject.SetActive(false);
            _dialogPresenter.HideOverviewDialog();

            _ui.OpenUpgrading -= ChangeToUpgradeState;
            _ui.OpenEvolving -= ChangeToEvolveState;
        }

        protected override void OnCancel()
        {
            Context.Input.DisableInput();
            Context.CloseSystemEvent.RaiseEvent();
        }

        private void ChangeToUpgradeState() => fsm.RequestStateChange(State.UPGRADING);

        private void ChangeToEvolveState() => fsm.RequestStateChange(State.EVOLVING);
    }
}