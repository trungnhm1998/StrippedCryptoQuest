using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class InputTransferAmount : StateBase
    {
        private readonly UIMetadTransferPanel _metaDTransferPanel;

        public InputTransferAmount(GameObject transferPanel)
        {
            _metaDTransferPanel = transferPanel.GetComponent<UIMetadTransferPanel>();
        }

        protected override void OnEnter()
        {
            _metaDTransferPanel.TransferAmountInput.Select();
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
        }

        protected override void OnExit()
        {
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
        }

        private void ToSelectTransferTypeState()
        {
            StateMachine.ChangeState(StateMachine.TransferringMetaDState);
        }
    }
}