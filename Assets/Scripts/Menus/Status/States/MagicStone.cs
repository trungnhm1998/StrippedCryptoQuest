using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.Status.States
{
    public class MagicStone : StatusStateBase
    {
        private TinyMessageSubscriptionToken _inventoryFilledEvent;

        public MagicStone(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            _inventoryFilledEvent = ActionDispatcher.Bind<StoneInventoryFilled>(GetStonesFromInventory);

            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());

            StatusPanel.MagicStoneMenu.SetActive(true);
        }

        private void GetStonesFromInventory(StoneInventoryFilled _)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            List<IMagicStone> stoneList = StatusPanel.StoneInventory.MagicStones;
            StatusPanel.StoneList.SetData(stoneList);
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
            ActionDispatcher.Unbind(_inventoryFilledEvent);
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.MagicStoneMenu.SetActive(false);
            fsm.RequestStateChange(State.OVERVIEW);
        }
    }
}