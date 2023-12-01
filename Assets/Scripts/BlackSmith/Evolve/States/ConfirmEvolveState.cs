using CryptoQuest.BlackSmith.Evolve.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class ConfirmEvolveState : EvolveStateBase
    {
        private ConfirmEvolvePresenter _confirmEvolvePresenter;

        public ConfirmEvolveState(EvolveStateMachine stateMachine) : base(stateMachine)
        {
            _evolveStateMachine = stateMachine;
            _evolveStateMachine.Presenter.TryGetComponent(out _confirmEvolvePresenter, true);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _confirmEvolvePresenter.gameObject.SetActive(true);
            _confirmEvolvePresenter.OnConfirmEvolving += HandleConfirmEvolving;
            _confirmEvolvePresenter.OnCancelEvolving += OnCancel;
        }

        public override void OnExit()
        {
            base.OnExit();

            _confirmEvolvePresenter.gameObject.SetActive(false);
            RemoveEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _confirmEvolvePresenter.OnConfirmEvolving -= HandleConfirmEvolving;
            _confirmEvolvePresenter.OnCancelEvolving -= OnCancel;
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            _evolveStateMachine.RequestStateChange(EvolveConstants.SELECT_MATERIAL_STATE);
        }

        private void HandleConfirmEvolving()
        {
            //TODO: Call API to evolve
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
                _evolveStateMachine.RequestStateChange(EvolveConstants.EVOLVE_SUCCESS_STATE);
            else
                _evolveStateMachine.RequestStateChange(EvolveConstants.EVOLVE_FAIL_STATE);
        }
    }
}
