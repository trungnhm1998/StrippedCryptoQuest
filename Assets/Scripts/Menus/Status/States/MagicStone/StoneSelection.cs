using CryptoQuest.Menus.Status.UI;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class StoneSelection : StatusStateBase
    {
        public StoneSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEntry;
            StatusPanel.MagicStoneSelection.EnterStoneSelection();
        }

        private void BackToEntry() => fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);


        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEntry;
            StatusPanel.MagicStoneSelection.ExitStoneSelection();
        }
    }
}