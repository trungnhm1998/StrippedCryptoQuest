using CryptoQuest.Core;
using TinyMessenger;

namespace CryptoQuest.DimensionalBox.States
{
    public class ConfirmTransferAction : ActionBase { }

    internal class ConfirmTransfer : StateBase
    {
        private TinyMessageSubscriptionToken _transferredEvent;

        protected override void OnEnter()
        {
            // TODO: Yes no dialog and raise transfer on yes

            StateMachine.Input.MenuCancelEvent += BackToSelectEquipmentsToTransfer;
            _transferredEvent = ActionDispatcher.Bind<TransferSucceed>(_ => BackToSelectEquipmentsToTransfer());
            ActionDispatcher.Dispatch(new ConfirmTransferAction());
        }

        protected override void OnExit()
        {
            ActionDispatcher.Unbind(_transferredEvent);
            StateMachine.Input.MenuCancelEvent -= BackToSelectEquipmentsToTransfer;
        }

        private void BackToSelectEquipmentsToTransfer()
        {
            StateMachine.ChangeState(StateMachine.TransferringEquipmentsState);
        }
    }
}