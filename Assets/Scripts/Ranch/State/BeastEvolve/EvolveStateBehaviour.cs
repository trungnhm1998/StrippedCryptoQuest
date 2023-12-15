using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastEvolve
{
    public class EvolveStateBehaviour : BaseStateBehaviour
    {        
        [SerializeField] private LocalizedString _message;
        [SerializeField] private LocalizedString _overviewMessage;
        private RanchStateController _controller;
        private static readonly int OverviewState = Animator.StringToHash("OverviewState");
        private static readonly int SelectMaterialState = Animator.StringToHash("EvolveSelectMaterialState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            _controller.Controller.Input.CancelEvent += CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent += ChangeSelectMaterialState;
            _controller.DialogController.NormalDialogue.SetMessage(_message).Show();
            _controller.EvolvePresenter.Init();
        }

        private void ChangeSelectMaterialState()
        {
            SelectBaseMaterial();
            _controller.DialogController.NormalDialogue.Hide();
            StateMachine.Play(SelectMaterialState);
        }

        private void SelectBaseMaterial()
        {
            var presenter = _controller.EvolvePresenter;
            presenter.BeastToEvolve = presenter.UIBeastEvolve.Beast;
            presenter.FilterBeastMaterial(presenter.UIBeastEvolve);
        }

        private void CancelBeastEvolveState()
        {
            _controller.UIBeastEvolve.Contents.SetActive(false);
            _controller.Controller.Initialize();
            _controller.DialogController.NormalDialogue.SetMessage(_overviewMessage).Show();
            StateMachine.Play(OverviewState);
        }

        protected override void OnExit()
        {
            _controller.Controller.Input.CancelEvent -= CancelBeastEvolveState;
            _controller.Controller.Input.SubmitEvent -= ChangeSelectMaterialState;
        }
    }
}