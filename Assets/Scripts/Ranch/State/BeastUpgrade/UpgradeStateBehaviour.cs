using CryptoQuest.Beast;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _welcomeMessage;
        [SerializeField] private LocalizedString _overviewMessage;

        private RanchStateController _stateController;
        private MerchantsInputManager _input;
        private UIDialogueForGenericMerchant _dialogue;

        private static readonly int OverviewState = Animator.StringToHash("OverviewState");
        private static readonly int SelectLevelState = Animator.StringToHash("SelectLevelState");


        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _input = _stateController.Controller.Input;
            _dialogue = _stateController.DialogController.NormalDialogue;
            _stateController.UIBeastUpgrade.Contents.SetActive(true);

            _input.CancelEvent += CancelBeastUpgradeState;
            _input.SubmitEvent += SelectBeastLevel;
            _dialogue.SetMessage(_welcomeMessage).Show();

            _stateController.UpgradePresenter.InitBeast(_stateController.BeastInventory.OwnedBeasts);
        }

        private void SelectBeastLevel()
        {
            StateMachine.Play(SelectLevelState);
        }

        private void CancelBeastUpgradeState()
        {
            _dialogue.SetMessage(_overviewMessage).Show();
            _stateController.Controller.ShowWalletEventChannel.Hide();
            _stateController.Controller.Initialize();

            _stateController.UIBeastUpgrade.Contents.SetActive(false);
            StateMachine.Play(OverviewState);
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= CancelBeastUpgradeState;
            _input.SubmitEvent -= SelectBeastLevel;

            _dialogue.Hide();

            _stateController.UpgradePresenter.Interactable = false;
        }
    }
}