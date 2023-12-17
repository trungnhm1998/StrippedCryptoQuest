using CryptoQuest.Menus.Status.UI;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class ElementNavigation : StatusStateBase
    {
        public ElementNavigation(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEntry;
            StatusPanel.MagicStoneSelection.EnterElementNavigation();
            StatusPanel.MagicStoneSelection.UIStoneList.ElementSelectedEvent += ToStoneSelection;
        }

        private void ToStoneSelection()
        {
            fsm.RequestStateChange(State.MAGIC_STONE_SELECTION);
        }

        private void BackToEntry() => fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEntry;
            StatusPanel.MagicStoneSelection.UIStoneList.ElementSelectedEvent -= ToStoneSelection;
        }
    }
}