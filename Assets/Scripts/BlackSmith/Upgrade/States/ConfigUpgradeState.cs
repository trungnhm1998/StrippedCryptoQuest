using CryptoQuest.BlackSmith.Upgrade.Presenters;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public class ConfigUpgradeState : UpgradeStateBase
    {
        public ConfigUpgradeState(UpgradeStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            ConfigUpgradePresenter.gameObject.SetActive(true);
            DialogsPresenter.Dialogue.SetMessage(UpgradeSystem.ConfigUpgradeText).Show();

            Input.NavigateEvent += HandleNavigation;
        }

        public override void OnExit()
        {
            base.OnExit();
            ConfigUpgradePresenter.gameObject.SetActive(false);

            Input.NavigateEvent -= HandleNavigation;
        }

        public override void OnCancel()
        {
            EquipmentDetailsPresenter.ResetPreviews();
            fsm.RequestStateChange(EStates.SelectEquipment);
        }

        public override void OnSubmit()
        {
            if (!ConfigUpgradePresenter.IsUpgradeValid)
            {
                Debug.LogWarning($"Not enough gold");
                return;
            }

            StateMachine.GoldNeeded = ConfigUpgradePresenter.GoldNeeded;
            StateMachine.LevelToUpgrade = ConfigUpgradePresenter.LevelToUpgrade;
            fsm.RequestStateChange(EStates.ConfirmUpgrade);
        }

        
        private void HandleNavigation(Vector2 direction)
        {
            ConfigUpgradePresenter.HandleNavigation(direction);
        }
    }
}