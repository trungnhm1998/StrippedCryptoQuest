using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;

namespace CryptoQuest.Menus.Item.States
{
    public class ItemSelectionState : ItemStateBase
    {
        private readonly InputMediatorSO _input;

        public ItemSelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
            _input = consumablePanel.Input;
        }

        public override void OnEnter()
        {
            ConsumablePanel.ItemConsumed += UseItem;

            _input.MenuCancelEvent += HandleCancel;
        }

        private void HandleCancel()
        {
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        public override void OnExit()
        {
            ConsumablePanel.ItemConsumed -= UseItem;

            _input.MenuCancelEvent -= HandleCancel;
        }

        private void UseItem()
        {
            fsm.RequestStateChange(ItemMenuStateMachine.ConsumingItem);
        }
    }
}