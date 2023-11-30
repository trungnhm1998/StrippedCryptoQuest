using CryptoQuest.BlackSmith.Interface;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class ConfigUpgradeState : UpgradingStateBase
    {
        private ConfigUpgradePresenter _configUpgradePresenter;

        public ConfigUpgradeState(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _upgradePresenter.TryGetComponent(out _configUpgradePresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _configUpgradePresenter.Show();

            _configUpgradePresenter.ConfiguratedUpgrade += ConfiguratedUpgrade;
        }

        public override void OnExit()
        {
            base.OnExit();
            _configUpgradePresenter.Hide();

            RemoveEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _configUpgradePresenter.ConfiguratedUpgrade -= ConfiguratedUpgrade;
        }

        protected override void OnCancel()
        {
            _configUpgradePresenter.CancelUI();
            fsm.RequestStateChange(Contants.SELECT_UPGRADE_STATE);
        }

        private void ConfiguratedUpgrade(IUpgradeEquipment equipment)
        {
            fsm.RequestStateChange(Contants.CONFIRM_UPGRADE_STATE);
        }
    }
}