using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States
{
    public class MagicStone : StatusStateBase
    {
        private TinyMessageSubscriptionToken _inventoryFilledEvent;
        private List<IMagicStone> _stoneList = new();

        public MagicStone(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
            _inventoryFilledEvent = ActionDispatcher.Bind<StoneInventoryFilled>(GetStonesFromInventory);

            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());

            StatusPanel.MagicStoneMenu.gameObject.SetActive(true);
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
            StatusPanel.MagicStoneMenu.gameObject.SetActive(false);
            fsm.RequestStateChange(State.EQUIPMENT_SELECTION);
        }
    }
}