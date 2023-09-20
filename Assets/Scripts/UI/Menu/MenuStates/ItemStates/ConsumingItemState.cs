using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class ConsumingItemState : ItemStateBase
    {
        public ConsumingItemState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            ConsumablePanel.ItemConsumed += BackToSelectItemState;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            ConsumablePanel.ItemConsumed -= BackToSelectItemState;
            fsm.RequestStateChange(ItemMenuStateMachine.ItemSelection);
        }
        
        private void BackToSelectItemState()
        {
            fsm.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }
    }
}