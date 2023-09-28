using System;
using CryptoQuest.UI.Menu.Panels.DimensionBox;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public class EquipmentTransferState : DimensionBoxStateBase
    {
        public EquipmentTransferState(UIDimensionBoxMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO);
            DimensionBoxPanel.EquipmentTransferSection.EnterTransferSection();
            DimensionBoxPanel.EquipmentTransferSection.SendingPhaseEvent += CheckIsOnSendingPhase;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(DimensionBoxMenuStateMachine.TransferTypeSelection);
            DimensionBoxPanel.EquipmentTransferSection.SendingPhaseEvent -= CheckIsOnSendingPhase;
        }

        private bool _isOnSendingPhase;
        private void CheckIsOnSendingPhase(bool isOnSendingPhase)
        {
            _isOnSendingPhase = isOnSendingPhase;
        }

        public override void OnExit()
        {
            base.OnExit();
            DimensionBoxPanel.EquipmentTransferSection.ExitTransferSection();
        }

        public override void Reset()
        {
            base.Reset();
            DimensionBoxPanel.EquipmentTransferSection.ResetTransfer();
        }

        public override void Execute()
        {
            base.Execute();
            DimensionBoxPanel.EquipmentTransferSection.SendItems();
        }

        public override void Interact()
        {
            base.Interact();
            DimensionBoxPanel.EquipmentTransferSection.OnInspectItem();
        }

        public override void HandleNavigate(Vector2 direction)
        {
            if (_isOnSendingPhase) return;
            if (direction.x != 0)
                DimensionBoxPanel.EquipmentTransferSection.OnSwitchBoard(direction);
        }
    }
}
