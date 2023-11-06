using CryptoQuest.Menus.Item.UI;
using FSM;

namespace CryptoQuest.Menus.Item.States
{
    public class ItemMenuStateMachine : StateMachine
    {
        public static readonly string InventorySelection = "InventorySelection";
        public static readonly string ItemConsume = "ItemConsume";

        public ItemMenuStateMachine(UIConsumableMenuPanel panel) : base(false)
        {
            AddState(InventorySelection, new InventorySelectionState(panel));
            AddState(ItemConsume, new ItemConsumeState(panel));

            SetStartState(InventorySelection);
        }
    }
}