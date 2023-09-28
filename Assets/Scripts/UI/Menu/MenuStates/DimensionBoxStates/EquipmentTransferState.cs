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
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(DimensionBoxMenuStateMachine.TransferTypeSelection);
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
            if (direction.x != 0)
                DimensionBoxPanel.EquipmentTransferSection.OnSwitchBoard(direction);
        }
    }
}
