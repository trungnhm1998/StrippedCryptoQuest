using CryptoQuest.UI.Menu.Panels.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class InventorySelectionState : ItemStateBase
    {
        public InventorySelectionState(UIConsumableMenuPanel consumablePanel) : base(consumablePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _consumablePanel.Interactable = true;
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(_consumablePanel.TypeSO);

            UIConsumableItem.Using += UseItem;
        }

        public override void OnExit()
        {
            base.OnExit();
            _consumablePanel.Interactable = false;
            UIConsumableItem.Using -= UseItem;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(_consumablePanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(ItemMenuStateMachine.NavItem);
        }

        public override void ChangeTab(float direction)
        {
            _consumablePanel.ChangeTab(direction);
        }

        private void UseItem(UIConsumableItem selectedConsumableItem)
        {
            fsm.RequestStateChange(ItemMenuStateMachine.ConsumingItem);
        }
    }
}