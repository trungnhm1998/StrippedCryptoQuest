using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class InventorySelectionState : ItemStateBase
    {
        public InventorySelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            ConsumablePanel.Interactable = true;
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(ConsumablePanel.TypeSO);

            UIConsumableItem.Using += UseItem;
        }

        public override void OnExit()
        {
            base.OnExit();
            ConsumablePanel.Interactable = false;
            UIConsumableItem.Using -= UseItem;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(ConsumablePanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.NavItem);
        }

        public override void ChangeTab(float direction)
        {
            ConsumablePanel.ChangeTab(direction);
        }

        private void UseItem(UIConsumableItem selectedConsumableItem)
        {
            // _interactingUI = selectedConsumableItem; // to refocus the item when the item usage menu is closed
            fsm.RequestStateChange(ItemMenuStateMachine.ConsumingItem);
        }
    }
}