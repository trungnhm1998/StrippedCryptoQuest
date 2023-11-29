using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Character.Hero;
using FSM;
using CryptoQuest.Menus.Home.UI;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.Home.States
{
    public class CharacterListState : StateBase
    {
        private UIHomeMenu _homePanel;
        private TinyMessageSubscriptionToken _fetchHeroesSucceeded;
        private List<HeroSpec> _heroes = new();

        public CharacterListState(UIHomeMenu panel) : base(false) => _homePanel = panel;

        public override void OnEnter()
        {
            _homePanel.InventoryFilled.EventRaised += LoadHeroesFromInventory;
            _homePanel.Input.MenuCancelEvent += HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(true);

            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
        }

        public override void OnExit()
        {
            _homePanel.InventoryFilled.EventRaised -= LoadHeroesFromInventory;
            _homePanel.Input.MenuCancelEvent -= HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(false);
        }

        private void LoadHeroesFromInventory(List<HeroSpec> heroes)
        {
            _homePanel.UICharacterList.SetData(heroes);
        }

        private void HandleCancel()
        {
            fsm.RequestStateChange(HomeMenuStateMachine.Overview);
        }
    }
}