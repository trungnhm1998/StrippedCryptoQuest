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
        private static readonly int EvolveState = Animator.StringToHash("EvolveState");
        private static readonly int ResultState = Animator.StringToHash("EvolveResultState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastEvolve.Contents.SetActive(true);
            _controller.Controller.Input.CancelEvent += NoButtonPressed;
            UpdateEvolvableInfo(_controller.EvolvePresenter.BeastToEvolve);
            SetupDialog();
        }

        private void SetupDialog()
        {
            _controller.DialogManager.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }

        private void YesButtonPressed()
        {
            _controller.DialogManager.ChoiceDialog.Hide();
            ChangeConfirmState();
        }

        private void NoButtonPressed()
        {
            _controller.DialogManager.ChoiceDialog.Hide();
            StateMachine.Play(EvolveState);
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.CancelEvent -= NoButtonPressed;
        }

        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new RequestEvolveBeast()
            {
                Base = _controller.EvolvePresenter.BeastToEvolve,
                Material = _controller.EvolvePresenter.BeastMaterial
            });
        }

        private void ChangeConfirmState()
        {
            StateMachine.Play(ResultState);
            HandleConfirmEvolving();
        }
        
        private void UpdateEvolvableInfo(IBeast beast)
        {
            var info = _controller.EvolvePresenter.BeastEvolvableInfos.First(f => f.BeforeStars == beast.Stars);
            _controller.EvolvePresenter.UpdateEvolvableInfo(info);
        }
    }
}