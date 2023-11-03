using System;
using CryptoQuest.Input;
using CryptoQuest.Menus.Item.UI;

namespace CryptoQuest.Menus.Item.States
{
    public class ConsumingItemState : ItemStateBase
    {
        public static event Action Cancelled;
        private readonly InputMediatorSO _input;

        public ConsumingItemState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
            _input = ConsumablePanel.Input;
        }

        public override void OnEnter()
        {
            ConsumablePanel.ItemConsumed += BackToSelectItemState;
            _input.MenuCancelEvent += HandleCancel;
        }

        private void HandleCancel()
        {
            Cancelled?.Invoke();
            BackToSelectItemState();
        }

        public override void OnExit()
        {
            ConsumablePanel.ItemConsumed -= BackToSelectItemState;
            _input.MenuCancelEvent -= HandleCancel;
        }

        private void BackToSelectItemState()
        {
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }
    }
}