using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class ItemSelectionState : ItemStateBase
    {
        private UIConsumables _overview;

        public ItemSelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(_consumablePanel.TypeSO);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(_consumablePanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        private void UseItem()
        {
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.CharacterSelection);
        }
    }
}