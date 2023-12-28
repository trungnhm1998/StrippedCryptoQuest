using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class InputTransferAmount : TransferMetadBaseState
    {
        public InputTransferAmount(TransferMetadStateMachine fsm) : base(fsm)
        {
            AddAction(EStateAction.OnCancel, () => fsm.RequestStateChange(EMetadState.SelectSource));
            AddAction(EStateAction.OnExecute, ToConfirmTransfer);
            AddAction(EStateAction.OnReset, () => fsm.RequestStateChange(EMetadState.SelectSource));
        }
    
        public override void OnEnter()
        {
            _panel.InputTransferUI.Select();
        }
    
        public override void OnExit()
        {
            _panel.InputTransferUI.DeSelect();
        }

        private void ToConfirmTransfer()
        {
            if (!_panel.IsInputValid) return;

            _fsm.TransferAmount = _panel.InputedValue;
            _fsm.RequestStateChange(EMetadState.ConfirmTransfer);
        }
    }
}