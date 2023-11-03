using CryptoQuest.Menus.Item.UI;
using FSM;

namespace CryptoQuest.Menus.Item.States
{
    public class ItemMenuStateMachine : StateMachine
    {
        public static readonly string InventorySelection = "InventorySelection";
        public static readonly string ItemSelection = "ItemSelection";
        public static readonly string ConsumingItem = "ConsumingItem";

        public ItemMenuStateMachine(UIConsumableMenuPanel panel) : base(false)
        {
            AddState(InventorySelection, new InventorySelectionState(panel));
            AddState(ItemSelection, new ItemSelectionState(panel));
            AddState(ConsumingItem, new ConsumingItemState(panel));

            SetStartState(InventorySelection);
        }
    }
}