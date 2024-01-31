using System.Linq;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Sagas;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveConfirmState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;

        private TinyMessageSubscriptionToken _evolveSuccessToken;
        private TinyMessageSubscriptionToken _evolveRequestFailedToken;
        private TinyMessageSubscriptionToken _evolveRequestSuccess;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<RanchStateController>();
            _stateController.UIBeastEvolve.Contents.SetActive(true);

            _input.CancelEvent += NoButtonPressed;

            _evolveSuccessToken = ActionDispatcher.Bind<BeastEvolveRespond>(HandleEvolveSuccess);
            _evolveRequestFailedToken = ActionDispatcher.Bind<EvolveRequestFailed>(HandleRequestFailed);
            _evolveRequestSuccess = ActionDispatcher.Bind<EvolveRequestSuccess>(UpdateBeastResult);

            UpdateEvolvableInfo(_stateController.EvolvePresenter.BeastToEvolve);
            SetupDialog();
        }

        private void UpdateBeastResult(EvolveRequestSuccess obj)
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(ResultState);
        }

        private void HandleEvolveSuccess(BeastEvolveRespond ctx)
        {
            var info = _stateController.EvolvePresenter.BeastEvolvableInfos.First(f =>
                f.BeforeStars == ctx.RequestContext.Base.Stars);
            _stateController.EvolvePresenter.UpdateCurrency(info);
            SetBeastID(ctx);
        }

        private void SetBeastID(BeastEvolveRespond ctx)
        {
            var response = ctx.Response;

            int evolveStatus = response.data.success;

            switch (evolveStatus)
            {
                case 0:
                    _stateController.BeastEvolveId = ctx.RequestContext.Base.Id;
                    _stateController.EvolveStatus = false;
                    break;
                case 1:
                    _stateController.BeastEvolveId = response.data.newBeast.id;
                    _stateController.EvolveStatus = true;
                    break;
                default:
                    Debug.LogError("[EvolveBeast]:: unknown success status: ");
                    break;
            }
        }

        private void HandleRequestFailed(EvolveRequestFailed obj)
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(EvolveState);
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= NoButtonPressed;
            ActionDispatcher.Unbind(_evolveSuccessToken);
            ActionDispatcher.Unbind(_evolveRequestSuccess);
            ActionDispatcher.Unbind(_evolveRequestFailedToken);
        }

        private void SetupDialog()
        {
            _stateController.DialogController.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }

        private void YesButtonPressed()
        {
            HandleConfirmEvolving();
            _stateController.DialogController.ChoiceDialog.Hide();
        }

        private void NoButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(EvolveState);
        }

        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new RequestEvolveBeast()
            {
                Base = _stateController.EvolvePresenter.BeastToEvolve,
                Material = _stateController.EvolvePresenter.BeastMaterial
            });
        }

        private void UpdateEvolvableInfo(IBeast beast)
        {
            var info = _stateController.EvolvePresenter.BeastEvolvableInfos.First(f => f.BeforeStars == beast.Stars);
            _stateController.EvolvePresenter.UpdateEvolvableInfo(info);
        }
    }
}