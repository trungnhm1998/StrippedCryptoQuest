using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using FSM;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class TransferMetadBaseState : ActionState<EMetadState, EStateAction>
    {
        protected readonly MetadTransferPanel _panel;
        protected TransferMetadStateMachine _fsm;
    
        public TransferMetadBaseState(TransferMetadStateMachine fsm) : base(false)
        {
            _fsm = fsm;
            _panel = fsm.Panel;
        }
    }
}