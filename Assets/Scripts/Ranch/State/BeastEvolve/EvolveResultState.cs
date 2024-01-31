using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveResultState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _evolveSucceededMessage;
        [SerializeField] private LocalizedString _evolveFailedMessage;
        [SerializeField] private LocalizedString _evolveSuccessTitle;
        [SerializeField] private LocalizedString _evolveFailTitle;
        private TinyMessageSubscriptionToken _getDataSucceed;

        protected override void OnEnter()
        {
            _input.CancelEvent += CancelBeastEvolveState;
            _input.SubmitEvent += ChangeConfirmState;

            _getDataSucceed = ActionDispatcher.Bind<GetBeastSucceeded>(UpdateResultStats);

            _stateController.Controller.ShowWalletEventChannel
                .EnableSouls(false)
                .Show();
        }

        protected override void OnExit()
        {
            _input.SubmitEvent -= ChangeConfirmState;
            _input.CancelEvent -= CancelBeastEvolveState;

            _stateController.DialogController.NormalDialogue.Hide();

            ActionDispatcher.Unbind(_getDataSucceed);
        }

        private void UpdateResultStats(ActionBase ctx)
        {
            var isSuccess = _stateController.EvolveStatus;
            var message = isSuccess ? _evolveSucceededMessage : _evolveFailedMessage;
            _dialogue.SetMessage(message).Show();

            var title = isSuccess ? _evolveSuccessTitle : _evolveFailTitle;
            int beastId = _stateController.BeastEvolveId;
            _stateController.EvolvePresenter.ShowBeastResult(title, beastId);
        }

        private void ChangeConfirmState()
        {
            StateMachine.Play(EvolveState);
        }

        private void CancelBeastEvolveState()
        {
            StateMachine.Play(EvolveState);
        }
    }
}