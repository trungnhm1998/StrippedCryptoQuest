using System;
using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;

namespace CryptoQuest.Menus.Item.States
{
    public class ItemConsumeState : ItemStateBase
    {
        public static event Action Cancelled;
        private readonly InputMediatorSO _input;

        public ItemConsumeState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
            _input = consumablePanel.Input;
        }

        public override void OnEnter()
        {
            DeselectAllHeroes();
            ConsumablePanel.Interactable = false;

            ConsumablePanel.SingleAlliedTarget.EventRaised += SelectSingleHero;
            ConsumablePanel.AllAlliesTarget.EventRaised += SelectAllHeroes;

            _input.MenuCancelEvent += HandleCancel;
        }

        public override void OnExit()
        {
            ConsumablePanel.SingleAlliedTarget.EventRaised -= SelectSingleHero;
            ConsumablePanel.AllAlliesTarget.EventRaised -= SelectAllHeroes;

            _input.MenuCancelEvent -= HandleCancel;

            ConsumablePanel.Interactable = true;
            DeselectAllHeroes();
        }

        private void HandleCancel()
        {
            Cancelled?.Invoke();
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        private void DeselectAllHeroes() => ConsumablePanel.EnableAllHeroButtons(false);

        private void SelectAllHeroes() => fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);

        private void SelectSingleHero() => SelectFirstHero();

        private void SelectFirstHero()
        {
            ConsumablePanel.EnableAllHeroButtons();
            ConsumablePanel.HeroButtons[0].Select();
        }
    }
}