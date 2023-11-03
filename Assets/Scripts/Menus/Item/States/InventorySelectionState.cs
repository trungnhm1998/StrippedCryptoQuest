using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;
using CryptoQuest.UI.Menu;
using FSM;
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
            ConsumablePanel.Interactable = true;
            ChangeTab(0);

            _input.MenuCancelEvent += HandleCancel;
            _input.TabChangeEvent += ChangeTab;

            UIConsumableItem.Using += UseItem;
        }

        public override void OnExit()
        {
            ConsumablePanel.Interactable = false;

            UIConsumableItem.Using -= UseItem;
            _input.MenuCancelEvent -= HandleCancel;
            UIConsumableItem.Using -= UseItem;
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }

        private void ChangeTab(float direction)
        {
            ConsumablePanel.ChangeTab(direction);
        }

        private void UseItem(UIConsumableItem selectedConsumableItem)
        {
            fsm.RequestStateChange(ItemMenuStateMachine.ItemSelection);
        }
    }
}