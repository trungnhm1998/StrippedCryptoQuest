using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastUpgrade
{
    public class UpgradeResultState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _resultMessage;


        protected override void OnEnter()
        {
            _stateController.UpgradePresenter.ShowResult();

            _input.CancelEvent += BackToUpgradeState;
            _input.SubmitEvent += BackToUpgradeState;

            _stateController.DialogController.NormalDialogue
                .SetMessage(_resultMessage)
                .Show();

            _stateController.Controller.ShowWalletEventChannel
                .EnableSouls(false)
                .Show();
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= BackToUpgradeState;
            _input.SubmitEvent -= BackToUpgradeState;
        }

        private void BackToUpgradeState()
        {
            _stateController.UpgradePresenter.HideResult();
            StateMachine.Play(UpgradeState);
        }
    }
}