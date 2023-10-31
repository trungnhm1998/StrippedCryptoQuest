using UnityEngine;

namespace CryptoQuest.DimensionalBox.States
{
    internal class TransferringMetaD : StateBase
    {
        private readonly GameObject _metaDTransferPanel;

        public TransferringMetaD(GameObject metaDTransferPanel)
        {
            _metaDTransferPanel = metaDTransferPanel;
        }

        protected override void OnEnter()
        {
            _metaDTransferPanel.SetActive(true);
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
        }

        protected override void OnExit()
        {
            _metaDTransferPanel.SetActive(false);
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
        }

        private void ToSelectTransferTypeState() => StateMachine.ChangeState(StateMachine.Landing);
    }
}