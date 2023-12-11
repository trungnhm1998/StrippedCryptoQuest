using CryptoQuest.Menus.Status.UI;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class SlotSelection : StatusStateBase
    {
        public SlotSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            StatusPanel.UIAttachList.SelectStoneToAttachEvent += ToNavigatingBetweenElements;
            StatusPanel.UIAttachList.EnterSlotSelection();
        }

        public override void OnExit()
        {
            StatusPanel.UIAttachList.ExitSlotSelection();
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
            StatusPanel.UIAttachList.SelectStoneToAttachEvent -= ToNavigatingBetweenElements;
        }

        private void ToNavigatingBetweenElements()
        {
            fsm.RequestStateChange(State.MAGIC_STONE_ELEMENT_NAVIGATION);
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.MagicStoneMenu.SetActive(false);
            StatusPanel.BackToPreviousState();
        }
    }
}