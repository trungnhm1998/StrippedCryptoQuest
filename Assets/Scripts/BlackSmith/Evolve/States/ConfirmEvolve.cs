using CryptoQuest.BlackSmith.Evolve.Sagas;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class ConfirmEvolve : EvolveStateBase
    {
        private TinyMessageSubscriptionToken _evolveSuccessToken;
        private TinyMessageSubscriptionToken _evolveFailedToken;

        public ConfirmEvolve(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.ConfirmEvolveText).Show();
            ConfirmEvolveDialog.gameObject.SetActive(true);
            ConfirmEvolveDialog.Confirmed += HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling += OnCancel;

            _evolveSuccessToken = ActionDispatcher.Bind<EvolveEquipmentSuccessAction>(HandleEvolveSuccess);
            _evolveFailedToken = ActionDispatcher.Bind<EvolveEquipmentFailedAction>(HandleEvolveFailed);
        }

        public override void OnExit()
        {
            base.OnExit();
            ConfirmEvolveDialog.Confirmed -= HandleConfirmEvolving;
            ConfirmEvolveDialog.Canceling -= OnCancel;
            ConfirmEvolveDialog.gameObject.SetActive(false);

            ActionDispatcher.Unbind(_evolveSuccessToken);
            ActionDispatcher.Unbind(_evolveFailedToken);
        }

        public override void OnCancel()
        {
            fsm.RequestStateChange(EStates.SelectMaterial);
        }

        private void HandleConfirmEvolving()
        {
            ActionDispatcher.Dispatch(new EvolveEquipmentAction()
            {
                EquipmentId = StateMachine.ItemToEvolve.Equipment.Id.ToString(),
                MaterialId = StateMachine.MaterialItem.Equipment.Id.ToString()
            });
        }

        private void HandleEvolveFailed(EvolveEquipmentFailedAction action)
        {
            fsm.RequestStateChange(EStates.EvolveFailed);
        }

        private void HandleEvolveSuccess(EvolveEquipmentSuccessAction action)
        {
            fsm.RequestStateChange(EStates.EvolveSuccess);
        }
    }
}
