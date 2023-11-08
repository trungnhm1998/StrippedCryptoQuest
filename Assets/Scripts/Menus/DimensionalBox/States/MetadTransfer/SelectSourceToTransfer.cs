using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States
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
            _metaDTransferPanel.GameButton.onClick.AddListener(FocusInputAmount);
            _metaDTransferPanel.DimensionalBoxButton.onClick.AddListener(FocusInputAmount);
        }

        protected override void OnExit()
        {
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
            _metaDTransferPanel.GameButton.onClick.RemoveListener(FocusInputAmount);
            _metaDTransferPanel.DimensionalBoxButton.onClick.RemoveListener(FocusInputAmount);
        }

        private void FocusInputAmount()
        {
            StateMachine.ChangeState(StateMachine.InputTransferAmount);
        }

        private void ToSelectTransferTypeState() => StateMachine.ChangeState(StateMachine.Landing);
    }
}