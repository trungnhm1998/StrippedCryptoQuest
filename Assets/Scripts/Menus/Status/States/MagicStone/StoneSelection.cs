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

        public StoneSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            _attachSucceededToken = ActionDispatcher.Bind<AttachSucceeded>(AttachSucceeded);
            StatusPanel.Input.MenuCancelEvent += BackToEntry;
            StatusPanel.MagicStoneSelection.EnterStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent += AttachStone;
        }

        private void AttachSucceeded(AttachSucceeded _)
        {
            StatusPanel.UIAttachList.AttachStoneToCurrentSlot(_stoneData);
            fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);
        }

        private void AttachStone(IMagicStone stoneData)
        {
            _stoneData = stoneData;

            List<int> stoneIDs = new();
            stoneIDs.Add(stoneData.ID);

            var equipmentDetails = StatusPanel.MagicStoneMenu.GetComponentInChildren<UIEquipmentDetails>();
            ActionDispatcher.Dispatch(new AttachStones()
            {
                EquipmentID = equipmentDetails.Equipment.Id,
                StoneIDs = stoneIDs
            });
        }

        private void BackToEntry() => fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEntry;
            StatusPanel.MagicStoneSelection.ExitStoneSelection();
            StatusPanel.MagicStoneSelection.UIStoneList.StoneSelectedEvent -= AttachStone;
            ActionDispatcher.Unbind(_attachSucceededToken);
        }
    }
}