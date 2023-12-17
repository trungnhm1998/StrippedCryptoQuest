using CryptoQuest.Menus.Status.UI;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class StoneSelection : StatusStateBase
    {
        public StoneSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEntry;
            StatusPanel.MagicStoneSelection.EnterStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent += AttachStone;
        }

        private void AttachStone()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void BackToEntry() => fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEntry;
            StatusPanel.MagicStoneSelection.ExitStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent -= AttachStone;
        }
    }
}