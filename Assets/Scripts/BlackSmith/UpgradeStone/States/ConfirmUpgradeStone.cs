using System.Linq;
using CryptoQuest.BlackSmith.UpgradeStone.Sagas;
using CryptoQuest.BlackSmith.UpgradeStone.UI;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.States
{
    public class ConfirmUpgradeStone : UpgradeMagicStoneStateBase
    {
        private TinyMessageSubscriptionToken _upgradeSuccessToken;
        private TinyMessageSubscriptionToken _upgradeFailedToken;
        private TinyMessageSubscriptionToken _requestUpgradeFailedToken;
        public ConfirmUpgradeStone(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            var confirmText = _stateMachine.UpgradeMagicStoneSystem.ConfirmUpgradeText;
            _dialogsPresenter.Dialogue.SetMessage(confirmText).Show();

            _confirmUpgradePresenter.gameObject.SetActive(true);
            _confirmUpgradePresenter.Confirmed += OnConfirmUpgrade;
            _confirmUpgradePresenter.Canceled += OnCancel;
            
            UpgradableStoneData stoneData = _stateMachine.StoneToUpgrade.MagicStoneData;
            _confirmUpgradePresenter.ConfirmStoneUpgradePanel.SetConfirmInfo(stoneData);
            _stateMachine.DialogsPresenter.ShowConfirmDialog(confirmText);

            _upgradeSuccessToken = ActionDispatcher.Bind<ResponseUpgradeStoneSuccess>(HandleUpdateSucceed);
            _upgradeFailedToken = ActionDispatcher.Bind<ResponseUpgradeStoneFailed>(HandleUpdateFailed);
            _requestUpgradeFailedToken = ActionDispatcher.Bind<RequestUpgradeStoneFailed>(HandleRequestFailed);
        }

        public override void OnExit()
        {
            base.OnExit();
            _confirmUpgradePresenter.Confirmed -= OnConfirmUpgrade;
            _confirmUpgradePresenter.Canceled -= OnCancel;
            _confirmUpgradePresenter.gameObject.SetActive(false);
            _stateMachine.DialogsPresenter.HideConfirmDialog();
            ActionDispatcher.Unbind(_upgradeSuccessToken);
            ActionDispatcher.Unbind(_upgradeFailedToken);
            ActionDispatcher.Unbind(_requestUpgradeFailedToken);
        }

        private void HandleUpdateFailed(ResponseUpgradeStoneFailed ctx)
        {
            fsm.RequestStateChange(EUpgradeMagicStoneStates.UpgradeFailed);
        }

        private void HandleRequestFailed(RequestUpgradeStoneFailed ctx)
        {
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectMaterialStone);
        }

        private void HandleUpdateSucceed(ResponseUpgradeStoneSuccess ctx)
        {
            _upgradeStoneResultPresenter.UpgradedStone = ctx.Stone;
            fsm.RequestStateChange(EUpgradeMagicStoneStates.UpgradeSucceed);
        }

        private void OnConfirmUpgrade()
        {
            var materialStones = _stateMachine.SelectedMaterials;

            ActionDispatcher.Dispatch(new RequestUpgradeStone(materialStones
                .Select(s => s.MagicStone).ToArray()));
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectMaterialStone);
        }
    }
}