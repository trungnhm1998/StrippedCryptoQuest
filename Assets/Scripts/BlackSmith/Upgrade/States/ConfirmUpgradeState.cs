using CryptoQuest.BlackSmith.Upgrade.Actions;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public class ConfirmUpgradeState : UpgradeStateBase
    {
        private TinyMessageSubscriptionToken _upgradeSuccessToken;
        private TinyMessageSubscriptionToken _upgradeFailToken;
        
        public ConfirmUpgradeState(UpgradeStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            UIConfirmDetails.gameObject.SetActive(true);
            UIConfirmDetails.SetupUI(StateMachine.LevelToUpgrade, StateMachine.GoldNeeded);

            DialogsPresenter.Dialogue.Hide();
            DialogsPresenter.ShowConfirmDialog(UpgradeSystem.ConfirmUpgradeText);

            DialogsPresenter.ConfirmYesEvent += ConfirmedUpgrade;
            DialogsPresenter.ConfirmNoEvent += OnCancel;
            _upgradeSuccessToken = ActionDispatcher.Bind<UpgradeSucceed>(UpgradeSucceed);
            _upgradeFailToken = ActionDispatcher.Bind<UpgradeFailed>(UpgradeFailed);
        }

        public override void OnExit()
        {
            base.OnExit();
            UIConfirmDetails.gameObject.SetActive(false);
            DialogsPresenter.HideConfirmDialog();

            DialogsPresenter.ConfirmYesEvent -= ConfirmedUpgrade;
            DialogsPresenter.ConfirmNoEvent -= OnCancel;            
            ActionDispatcher.Unbind(_upgradeSuccessToken);
            ActionDispatcher.Unbind(_upgradeFailToken);
        }

        public override void OnCancel() => fsm.RequestStateChange(EStates.ConfigUpgrade);

        private void ConfirmedUpgrade()
        {
            ActionDispatcher.Dispatch(new RequestUpgrade(StateMachine.EquipmentToUpgrade,
                StateMachine.LevelToUpgrade));
        }

        private void UpgradeSucceed(UpgradeSucceed ctx)
        {
            fsm.RequestStateChange(EStates.UpgradeResult);
        }

        private void UpgradeFailed(UpgradeFailed ctx)
        {
            EquipmentsPresenter.CancelUI();
            fsm.RequestStateChange(EStates.SelectEquipment);
        }
    }
}