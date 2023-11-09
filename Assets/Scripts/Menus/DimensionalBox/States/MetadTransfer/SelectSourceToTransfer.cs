using CryptoQuest.Core;
using CryptoQuest.Menus.DimensionalBox.Actions;
using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class SelectSourceToTransfer : StateBase
    {
        private readonly UIMetadTransferPanel _metaDTransferPanel;

        public SelectSourceToTransfer(GameObject transferPanel)
        {
            _metaDTransferPanel = transferPanel.GetComponent<UIMetadTransferPanel>();
        }

        protected override void OnEnter()
        {
            _metaDTransferPanel.gameObject.SetActive(true);
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
            _metaDTransferPanel.TransferSourceChanged += FocusInputAmount;
            _metaDTransferPanel.SelectDefaultButton();
            
            ActionDispatcher.Dispatch(new GetToken());
        }

        protected override void OnExit()
        {
            _metaDTransferPanel.TransferSourceChanged -= FocusInputAmount;
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
        }

        private void FocusInputAmount()
        {
            StateMachine.ChangeState(StateMachine.InputTransferAmount);
        }

        private void ToSelectTransferTypeState() => StateMachine.ChangeState(StateMachine.Landing);
    }
}