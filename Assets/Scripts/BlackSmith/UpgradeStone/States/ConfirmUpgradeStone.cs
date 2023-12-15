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
        public ConfirmUpgradeStone(UpgradeMagicStoneStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            var confirmText = _stateMachine.UpgradeMagicStoneSystem.ConfirmUpgradeText;
            _dialogsPresenter.Dialogue.SetMessage(confirmText).Show();

            UpgradableStoneData stoneData =
                _stateMachine.UpgradeMagicStoneSystem.GetUpgradableStoneData(_stateMachine.StoneToUpgrade.MagicStone);
            _confirmUpgradePresenter.gameObject.SetActive(true);
            _confirmUpgradePresenter.Confirmed += OnConfirmUpgrade;
            _confirmUpgradePresenter.Canceled += OnCancel;
            _confirmUpgradePresenter.ConfirmStoneUpgradePanel.SetConfirmInfo(stoneData);
            _stateMachine.DialogsPresenter.ShowConfirmDialog(confirmText);

            _upgradeSuccessToken = ActionDispatcher.Bind<ResponseUpgradeStoneSuccess>(HandleUpdateSucceed);
            _upgradeFailedToken = ActionDispatcher.Bind<ResponseUpgradeStoneFailed>(HandleUpdateFailed);
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
        }

        private void HandleUpdateFailed(ResponseUpgradeStoneFailed ctx) { }
        private void HandleUpdateSucceed(ResponseUpgradeStoneSuccess ctx) { }

        private void OnConfirmUpgrade()
        {
            var materialStones = _stateMachine.SelectedMaterials;

            ActionDispatcher.Dispatch(new RequestUpgradeStone(materialStones.Select(s => s.MagicStone.ID).ToArray()));

            Debug.Log("UpgradeStonePresenter: Upgrade Stone request sent with ids" + materialStones[0].MagicStone.ID +
                      ", " +
                      materialStones[1].MagicStone.ID +
                      " " +
                      materialStones[2].MagicStone.ID);
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EUpgradeMagicStoneStates.SelectMaterialStone);
        }
    }
}