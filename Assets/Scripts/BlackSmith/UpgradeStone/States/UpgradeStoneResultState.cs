namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class UpgradeStoneResultState : UpgradeMagicStoneStateBase
    {
        public UpgradeStoneResultState(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _stoneUpgradePresenter.gameObject.SetActive(false);
            _upgradeStoneResultPresenter.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _dialogsPresenter.Dialogue.Hide();
            _upgradeStoneResultPresenter.gameObject.SetActive(false);
        }

        public override void OnSubmit()
        {
            base.OnSubmit();
            fsm.RequestStateChange(EUpgradeMagicStoneStates.LoadStone);
        }

        public override void OnCancel()
        {
            base.OnCancel();
            fsm.RequestStateChange(EUpgradeMagicStoneStates.LoadStone);
        }
    }
}