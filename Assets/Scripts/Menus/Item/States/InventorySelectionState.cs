using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Item.States
{
    public class InventorySelectionState : ItemStateBase
    {
        private readonly InputMediatorSO _input;

        public InventorySelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
            _input = ConsumablePanel.Input;
        }

        public override void OnEnter()
        {
            ConsumablePanel.Focusing += SelectFirstTab;

            _input.MenuCancelEvent += HandleCancel;
            _input.TabChangeEvent += ChangeTab;

            UIConsumableItem.Using += UseItem;
        }

        public override void OnExit()
        {
            _input.MenuCancelEvent -= HandleCancel;
            _input.TabChangeEvent -= ChangeTab;

            UIConsumableItem.Using -= UseItem;
        }

        private void SelectFirstTab()
        {
            ConsumablePanel.Interactable = true;

            ConsumablePanel.ShowItemsWithType(0);
            ChangeTab(0);
        }

        private void HandleCancel()
        {
            ConsumablePanel.Interactable = false;

            UIMainMenu.OnBackToNavigation();
        }

        private void ChangeTab(float direction)
        {
            ConsumablePanel.ChangeTab(direction);
        }

        private void UseItem(UIConsumableItem selectedConsumableItem)
        {
            fsm.RequestStateChange(ItemMenuStateMachine.ItemConsume);
        }
    }
}