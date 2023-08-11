using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class InventorySelectionState : ItemStateBase
    {

        public InventorySelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(ConsumablePanel.TypeSO);
            // _overview.InventorySelected += ViewItems;
            // _overview.Init();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(ConsumablePanel.TypeSO, true);
            // _overview.DeInit();
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.NavItem);
        }

        public override void OnExit()
        {
            base.OnExit();
            // _overview.InventorySelected -= ViewItems;
            // _overview.DeInit();
        }

        private void ViewItems()
        {
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.ItemSelection);
        }
    }
}