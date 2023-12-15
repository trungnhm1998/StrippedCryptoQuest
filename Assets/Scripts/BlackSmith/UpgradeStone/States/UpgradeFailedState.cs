namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class UpgradeFailedState : UpgradeStoneResultState
    {
        public UpgradeFailedState(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.UpgradeFailedText).Show();
            _upgradeStoneResultPresenter.SetResultFail();
        }
    }
}