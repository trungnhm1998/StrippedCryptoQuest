using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class ConfirmEvolve : EvolveStateBase
    {
        public ConfirmEvolve(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.ConfirmEvolveText).Show();
            ConfirmEvolveDialog.gameObject.SetActive(true);
            ConfirmEvolveDialog.Confirmed += HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling += OnCancel;
        }

        public override void OnExit()
        {
            base.OnExit();
            ConfirmEvolveDialog.Confirmed -= HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling -= OnCancel;
            ConfirmEvolveDialog.gameObject.SetActive(false);
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EStates.SelectMaterial);
        }

        private void HandleConfirmEvolving()
        {
            //TODO: Call API to evolve
            fsm.RequestStateChange(Random.Range(0, 2) == 0 ? EStates.EvolveSuccess : EStates.EvolveFailed);
        }
    }
}