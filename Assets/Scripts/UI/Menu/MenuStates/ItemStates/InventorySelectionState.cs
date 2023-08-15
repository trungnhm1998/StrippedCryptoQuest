using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class InventorySelectionState : ItemStateBase
    {
        public InventorySelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(ConsumablePanel.TypeSO);
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

        private void ViewItems()
        {
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.ItemSelection);
        }
    }
}