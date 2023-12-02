using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.States;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class ConfigUpgradeState : BlackSmithStateBase
    {
        private readonly ConfigUpgradePresenter _configUpgradePresenter;

        public ConfigUpgradeState(BlackSmithSystem context) : base(context)
        {
            context.UpgradePresenter.TryGetComponent(out _configUpgradePresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _configUpgradePresenter.Show();

            _configUpgradePresenter.ConfiguratedUpgrade += ToConfirmUpgrade;
        }

        public override void OnExit()
        {
            base.OnExit();
            _configUpgradePresenter.Hide();
            _configUpgradePresenter.ConfiguratedUpgrade -= ToConfirmUpgrade;
        }

        protected override void OnCancel()
        {
            _configUpgradePresenter.CancelUI();
            fsm.RequestStateChange(BlackSmith.State.SELECT_UPGRADE);
        }

        private void ToConfirmUpgrade(IUpgradeEquipment equipment)
        {
            fsm.RequestStateChange(BlackSmith.State.CONFIRM_UPGRADE);
        }
    }
}