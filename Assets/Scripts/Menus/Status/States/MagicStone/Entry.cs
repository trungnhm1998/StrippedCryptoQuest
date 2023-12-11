using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.Status.States.MagicStone
{
    public class Entry : StatusStateBase
    {
        private TinyMessageSubscriptionToken _inventoryFilledEvent;

        public Entry(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            _inventoryFilledEvent = ActionDispatcher.Bind<StoneInventoryFilled>(GetStonesFromInventory);

            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());

            var equipmentDetails = StatusPanel.MagicStoneMenu.GetComponentInChildren<UIEquipmentDetails>(true);
            equipmentDetails.Init(StatusPanel.InspectingEquipment);
        }

        private void GetStonesFromInventory(StoneInventoryFilled _)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            StatusPanel.MagicStoneMenu.SetActive(true);
            fsm.RequestStateChange(State.MAGIC_STONE_SLOT_SELECTION);
        }

        public override void OnExit()
        {
            ActionDispatcher.Unbind(_inventoryFilledEvent);
        }
    }
}