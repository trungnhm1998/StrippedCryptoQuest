using CryptoQuest.BlackSmith.Evolve.Sagas;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class ConfirmEvolve : EvolveStateBase
    {
        private TinyMessageSubscriptionToken _evolveSuccessToken;
        private TinyMessageSubscriptionToken _evolveFailedToken;
        private TinyMessageSubscriptionToken _evolveRequestFailedToken;

        public ConfirmEvolve(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.ConfirmEvolveText).Show();
            ConfirmEvolveDialog.gameObject.SetActive(true);
            ConfirmEvolveDialog.Confirmed += HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling += OnCancel;

            ConfirmEvolveDialog.ConfirmEvolveUI.SetConfirmInfo(StateMachine.EvolveEquipmentData);

            StateMachine.MaterialItem.MaterialTag.SetActive(true);
            StateMachine.MaterialItem.SetSelected(true);
            
            _evolveSuccessToken = ActionDispatcher.Bind<EvolveEquipmentSuccessAction>(HandleEvolveSuccess);
            _evolveFailedToken = ActionDispatcher.Bind<EvolveEquipmentFailedAction>(HandleEvolveFailed);
            _evolveRequestFailedToken = ActionDispatcher.Bind<EvolveRequestFailed>(HandleRequestFailed);
        }

        public override void OnExit()
        {
            base.OnExit();
            StateMachine.MaterialItem.ResetItemStates();

            ConfirmEvolveDialog.Confirmed -= HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling -= OnCancel;
            ConfirmEvolveDialog.gameObject.SetActive(false);

            ActionDispatcher.Unbind(_evolveSuccessToken);
            ActionDispatcher.Unbind(_evolveFailedToken);
            ActionDispatcher.Unbind(_evolveRequestFailedToken);
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EStates.SelectMaterial);
        }

        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new RequestEvolveEquipment()
            {
                Equipment = StateMachine.ItemToEvolve.Equipment,
                Material = StateMachine.MaterialItem.Equipment
            });
        }

        private void HandleEvolveFailed(EvolveEquipmentFailedAction ctx)
        {
            EvolveSystem.EvolveResultPresenter.SetResultFail(ctx.Equipment);
            fsm.RequestStateChange(EStates.EvolveFailed);
        }

        private void HandleEvolveSuccess(EvolveEquipmentSuccessAction ctx)
        {
            EvolveSystem.EvolveResultPresenter.SetResultSuccess(ctx.Equipment);
            fsm.RequestStateChange(EStates.EvolveSuccess);
        }

        private void HandleRequestFailed(EvolveRequestFailed ctx)
        {
            fsm.RequestStateChange(EStates.SelectEquipment);
        }
    }
}
