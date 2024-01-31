using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        [SerializeField] private LocalizedString _overviewMessage;

        protected override void OnEnter()
        {
            _stateController.UIBeastEvolve.Contents.SetActive(true);

            _input.CancelEvent += CancelBeastEvolveState;
            _input.SubmitEvent += ChangeSelectMaterialState;

            _stateController.EvolvePresenter.Init();
            _stateController.DialogController.NormalDialogue.SetMessage(_message).Show();
            _stateController.Controller.ShowWalletEventChannel.EnableAll().Show();
        }

        private void ChangeSelectMaterialState()
        {
            if (!_stateController.EvolvePresenter.UIBeastEvolve.IsEnoughCurrencies) return;
            SelectBaseMaterial();
            _stateController.DialogController.NormalDialogue.Hide();
            StateMachine.Play(SelectMaterialState);
        }

        private void SelectBaseMaterial()
        {
            var presenter = _stateController.EvolvePresenter;
            presenter.BeastToEvolve = presenter.UIBeastEvolve.Beast;
            presenter.FilterBeastMaterial(presenter.UIBeastEvolve);
        }

        private void CancelBeastEvolveState()
        {
            _stateController.UIBeastEvolve.Contents.SetActive(false);
            _stateController.Controller.Initialize();
            _stateController.DialogController.NormalDialogue.SetMessage(_overviewMessage).Show();
            StateMachine.Play(OverviewState);
            _stateController.Controller.ShowWalletEventChannel.Hide();
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= CancelBeastEvolveState;
            _input.SubmitEvent -= ChangeSelectMaterialState;
        }
    }
}