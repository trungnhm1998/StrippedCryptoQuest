using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States
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
        }

        protected override void OnExit() { }
    }
}