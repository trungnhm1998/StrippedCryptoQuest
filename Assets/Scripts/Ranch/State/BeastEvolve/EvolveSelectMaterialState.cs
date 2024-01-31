using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveSelectMaterialState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;

        protected override void OnEnter()
        {
            _stateController.UIBeastEvolve.Contents.SetActive(true);

            _input.CancelEvent += CancelBeastEvolveState;
            _input.SubmitEvent += SelectBeastMaterial;

            _stateController.DialogController.NormalDialogue.SetMessage(_message).Show();
        }

        protected override void OnExit()
        {
            _input.SubmitEvent -= SelectBeastMaterial;
            _input.CancelEvent -= CancelBeastEvolveState;

            _stateController.DialogController.NormalDialogue.Hide();
        }

        private void SelectBeastMaterial()
        {
            if (!_stateController.EvolvePresenter.UIBeastEvolve.IsEnoughCurrencies) return;
            var evolvePresenter = _stateController.EvolvePresenter;
            var uiBeastEvolve = _stateController.EvolvePresenter.UIBeastEvolve;

            evolvePresenter.BeastMaterial = uiBeastEvolve.Beast;

            if (evolvePresenter.BeastMaterial != evolvePresenter.BeastToEvolve)
            {
                evolvePresenter.UIBeastEvolve.SetMaterialObjectSelected(true);
                StateMachine.Play(EvolveConfirmState);
            }
        }

        private void CancelBeastEvolveState()
        {
            StateMachine.Play(EvolveState);
        }
    }
}