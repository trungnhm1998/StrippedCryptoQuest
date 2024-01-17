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
        private RanchStateController _controller;
        private TinyMessageSubscriptionToken _evolveSuccessToken;
        private TinyMessageSubscriptionToken _evolveRequestFailedToken;
        private TinyMessageSubscriptionToken _evolveRequestSuccess;
        private static readonly int EvolveState = Animator.StringToHash("EvolveState");
        private static readonly int ResultState = Animator.StringToHash("EvolveResultState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            _controller.UIBeastEvolve.Contents.SetActive(true);
            _controller.Controller.Input.CancelEvent += NoButtonPressed;

            _evolveSuccessToken = ActionDispatcher.Bind<BeastEvolveRespond>(HandleEvolveSuccess);
            _evolveRequestFailedToken = ActionDispatcher.Bind<EvolveRequestFailed>(HandleRequestFailed);
            _evolveRequestSuccess = ActionDispatcher.Bind<EvolveRequestSuccess>(UpdateBeastResult);

            UpdateEvolvableInfo(_controller.EvolvePresenter.BeastToEvolve);
            SetupDialog();
        }

        private void UpdateBeastResult(EvolveRequestSuccess obj)
        {
            _controller.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(ResultState);
        }

        private void HandleEvolveSuccess(BeastEvolveRespond ctx)
        {
            var info = _controller.EvolvePresenter.BeastEvolvableInfos.First(f =>
                f.BeforeStars == ctx.RequestContext.Base.Stars);
            _controller.EvolvePresenter.UpdateCurrency(info);
            SetBeastID(ctx);
        }

        private void SetBeastID(BeastEvolveRespond ctx)
        {
            var response = ctx.Response;

            int evolveStatus = response.data.success;

            switch (evolveStatus)
            {
                case 0:
                    _controller.BeastEvolveId = ctx.RequestContext.Base.Id;
                    _controller.EvolveStatus = false;
                    break;
                case 1:
                    _controller.BeastEvolveId = response.data.newBeast.id;
                    _controller.EvolveStatus = true;
                    break;
                default:
                    Debug.LogError("[EvolveBeast]:: unknown success status: ");
                    break;
            }
        }

        private void HandleRequestFailed(EvolveRequestFailed obj)
        {
            _controller.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(EvolveState);
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.CancelEvent -= NoButtonPressed;
            ActionDispatcher.Unbind(_evolveSuccessToken);
            ActionDispatcher.Unbind(_evolveRequestSuccess);
            ActionDispatcher.Unbind(_evolveRequestFailedToken);
        }

        private void SetupDialog()
        {
            _controller.DialogController.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }

        private void YesButtonPressed()
        {
            HandleConfirmEvolving();
            _controller.DialogController.ChoiceDialog.Hide();
        }

        private void NoButtonPressed()
        {
            _controller.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(EvolveState);
        }

        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new RequestEvolveBeast()
            {
                Base = _controller.EvolvePresenter.BeastToEvolve,
                Material = _controller.EvolvePresenter.BeastMaterial
            });
        }

        private void UpdateEvolvableInfo(IBeast beast)
        {
            var info = _controller.EvolvePresenter.BeastEvolvableInfos.First(f => f.BeforeStars == beast.Stars);
            _controller.EvolvePresenter.UpdateEvolvableInfo(info);
        }
    }
}