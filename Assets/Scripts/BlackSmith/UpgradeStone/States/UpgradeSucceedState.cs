using CryptoQuest.Item.MagicStone;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class UpgradeSucceedState : UpgradeStoneResultState
    {
        public UpgradeSucceedState(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _dialogsPresenter.Dialogue.SetMessage(_stateMachine.UpgradeMagicStoneSystem.UpgradeSuccessText).Show();
            _upgradeStoneResultPresenter.SetResultSuccess();
        }
    }
}