using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States
{
    public class MagicStone : StatusStateBase
    {
        private TinyMessageSubscriptionToken _inventoryFilledEvent;
        private MagicStoneInventorySo _stoneInventory;

        public MagicStone(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            _inventoryFilledEvent = ActionDispatcher.Bind<StoneInventoryFilled>(GetStonesFromInventory);
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
        }

        private void GetStonesFromInventory(StoneInventoryFilled _) { }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
            ActionDispatcher.Unbind(_inventoryFilledEvent);
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.ShowMagicStone.RaiseEvent(false);
            fsm.RequestStateChange(State.EQUIPMENT_SELECTION);
        }
    }
}