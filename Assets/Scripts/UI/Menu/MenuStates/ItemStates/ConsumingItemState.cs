using System;
using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class ConsumingItemState : ItemStateBase
    {
        public static event Action Cancelled;
        public ConsumingItemState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            ConsumablePanel.ItemConsumed += BackToSelectItemState;
        }

        public override void HandleCancel()
        {
            Cancelled?.Invoke();
            base.HandleCancel();
            ConsumablePanel.ItemConsumed -= BackToSelectItemState;
            BackToSelectItemState();
        }

        private void BackToSelectItemState()
        {
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }
    }
}