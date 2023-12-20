using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveSelectMaterialState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private RanchStateController _controller;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");
        private static readonly int ConfirmState = Animator.StringToHash("EvolveConfirmState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastEvolve.Contents.SetActive(true);

            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += SelectBeastMaterial;
            _controller.DialogController.NormalDialogue.SetMessage(_message).Show();
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.SubmitEvent -= SelectBeastMaterial;
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
            _controller.DialogController.NormalDialogue.Hide();
        }

        private void SelectBeastMaterial()
        {
            if (!_controller.EvolvePresenter.UIBeastEvolve.IsEnoughCurrencies) return;
            var evolvePresenter = _controller.EvolvePresenter;
            var uiBeastEvolve = _controller.EvolvePresenter.UIBeastEvolve;

            evolvePresenter.BeastMaterial = uiBeastEvolve.Beast;

            if (evolvePresenter.BeastMaterial != evolvePresenter.BeastToEvolve)
            {
                evolvePresenter.UIBeastEvolve.SetMaterialObjectSelected(true);
                StateMachine.Play(ConfirmState);
            }
        }

        private void CancelBeastEvolveState()
        {
            StateMachine.Play(EvolveState);
        }
    }
}