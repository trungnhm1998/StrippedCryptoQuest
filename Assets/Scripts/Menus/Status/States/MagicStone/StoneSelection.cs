using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.MagicStone;
using CryptoQuest.Sagas.Equipment;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class StoneSelection : StatusStateBase
    {
        private TinyMessageSubscriptionToken _attachSucceededToken;
        private IMagicStone _stoneData;

        public StoneSelection(UIStatusMenu statusPanel) : base(statusPanel)
        {
        }

        public override void OnEnter()
        {
            _attachSucceededToken = ActionDispatcher.Bind<AttachSucceeded>(AttachSucceeded);
            StatusPanel.Input.MenuCancelEvent += BackToSelectSlot;
            StatusPanel.MagicStoneSelection.EnterStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent += AttachStone;
        }

        private void AttachSucceeded(AttachSucceeded _)
        {
            StatusPanel.UIAttachList.AttachStoneToCurrentSlot(_stoneData);
            StatusPanel.MagicStoneSelection.UIStoneList.RenderAll();
            BackToSelectSlot();
        }

        private void AttachStone(IMagicStone stoneData)
        {
            _stoneData = stoneData;

            List<int> stoneIDs = new();
            stoneIDs.Add(stoneData.ID);

            ActionDispatcher.Dispatch(new AttachStones()
            {
                EquipmentID = StatusPanel.EquipmentDetails.Equipment.Id,
                StoneIDs = stoneIDs
            });
        }

        private void BackToSelectSlot() => fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToSelectSlot;
            StatusPanel.MagicStoneSelection.ExitStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent -= AttachStone;
            ActionDispatcher.Unbind(_attachSucceededToken);
        }
    }
}