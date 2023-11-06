using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;
using CryptoQuest.UI.Menu.Character;

namespace CryptoQuest.Menus.Item.States
{
    public class ItemConsumeState : ItemStateBase
    {
        public static event Action Cancelled;
        private readonly InputMediatorSO _input;

        private List<HeroBehaviour> _targets;

        public ItemConsumeState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
            _input = consumablePanel.Input;
        }

        public override void OnEnter()
        {
            foreach (var heroButton in _consumablePanel.HeroButtons) heroButton.Selecting += CacheLastSelectingSlot;

            DeselectAllHeroes();
            _consumablePanel.Interactable = false;

            _consumablePanel.SingleAlliedTarget.EventRaised += SelectSingleHero;
            _consumablePanel.AllAlliesTarget.EventRaised += SelectAllHeroes;

            _input.MenuCancelEvent += HandleCancel;
        }


        public override void OnExit()
        {
            foreach (var heroButton in _consumablePanel.HeroButtons) heroButton.Selecting -= CacheLastSelectingSlot;

            _consumablePanel.SingleAlliedTarget.EventRaised -= SelectSingleHero;
            _consumablePanel.AllAlliesTarget.EventRaised -= SelectAllHeroes;

            _input.MenuCancelEvent -= HandleCancel;

            _consumablePanel.Interactable = true;
            DeselectAllHeroes();
        }

        private void HandleCancel()
        {
            Cancelled?.Invoke();
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        private void CacheLastSelectingSlot(UICharacterPartySlot hero) => _consumablePanel.SelectingHero = hero;

        private void DeselectAllHeroes() => _consumablePanel.EnableAllHeroButtons(false);

        private void SelectAllHeroes() => fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);

        private void SelectSingleHero()
        {
            foreach (var heroButton in _consumablePanel.HeroButtons) heroButton.Selected -= UsingItem;

            _consumablePanel.EnableAllHeroButtons();
            _consumablePanel.HeroButtons[0].Select();
            _consumablePanel.SelectingHero.EnableSelectBackground();
        }

        private void UsingItem(UICharacterPartySlot uiCharacterPartySlot)
        {
            foreach (var heroButton in _consumablePanel.HeroButtons) heroButton.Selected -= UsingItem;
            
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }
    }
}