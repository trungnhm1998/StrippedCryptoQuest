using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class FocusItemState : ItemStateBase
    {
        public FocusItemState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

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
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.ItemSelection);
        }
    }
}