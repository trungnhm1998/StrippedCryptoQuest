using CryptoQuest.Ranch.Evolve.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveSelectMaterialState : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private static readonly int EvolveState = Animator.StringToHash("EvolveState");
        private static readonly int ConfirmState = Animator.StringToHash("EvolveConfirmState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _controller.UIBeastEvolve.Contents.SetActive(true);

            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += SelectBeastMaterial;
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.SubmitEvent -= SelectBeastMaterial;
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
        }

        private void SelectBeastMaterial()
        {
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