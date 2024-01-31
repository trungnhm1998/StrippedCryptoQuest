using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Beast;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _welcomeMessage;
        [SerializeField] private LocalizedString _overviewMessage;

        protected override void OnEnter()
        {
            _stateController.UIBeastUpgrade.Contents.SetActive(true);

            _stateController.Controller.ShowWalletEventChannel
                .EnableSouls(false)
                .Show();

            _input.CancelEvent += CancelBeastUpgradeState;
            _input.SubmitEvent += SelectBeastLevel;
            _dialogue.SetMessage(_welcomeMessage).Show();

            _stateController.UpgradePresenter.InitBeast(_stateController.BeastInventory.OwnedBeasts);
            _stateController.UpgradePresenter.ActiveBeastDetail(IsBeastValid());
        }


        protected override void OnExit()
        {
            _input.CancelEvent -= CancelBeastUpgradeState;
            _input.SubmitEvent -= SelectBeastLevel;

            _stateController.UpgradePresenter.Interactable = false;
            _stateController.DialogController.NormalDialogue.Hide();
        }

        private void SelectBeastLevel()
        {
            if (!IsBeastValid() && !CanUpgrade()) return;

            StateMachine.Play(SelectLevelState);
        }

        private void CancelBeastUpgradeState()
        {
            _dialogue.SetMessage(_overviewMessage).Show();

            _stateController.Controller.Initialize();
            _stateController.Controller.ShowWalletEventChannel.Hide();
            _stateController.UIBeastUpgrade.Contents.SetActive(false);

            _dialogue.Hide();
            StateMachine.Play(OverviewState);
        }

        private bool IsBeastValid()
        {
            List<IBeast> ownedBeasts = _stateController.BeastInventory.OwnedBeasts;
            return ownedBeasts.Any(beast => beast.Level < beast.MaxLevel);
        }

        private bool CanUpgrade()
        {
            return _stateController.UpgradePresenter.Interactable;
        }
    }
}