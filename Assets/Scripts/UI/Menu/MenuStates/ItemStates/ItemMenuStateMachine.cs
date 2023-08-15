using CryptoQuest.UI.Menu.Panels.Item;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class ItemMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavItem = "NavItem";
        public static readonly string Item = "Item";
        public static readonly string InventorySelection = "InventorySelection";
        public static readonly string ItemSelection = "ItemSelection";
        public static readonly string CharacterSelection = "CharacterSelection";

        /// <summary>
        /// Setup the state machine for Item menu.
        /// </summary>
        /// <param name="panel"></param>
        public ItemMenuStateMachine(UIConsumableMenuPanel panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavItem, new GenericUnfocusState(InventorySelection));
            AddState(Item, new FocusItemState(panel));
            AddState(InventorySelection, new InventorySelectionState(panel));
            AddState(ItemSelection, new ItemSelectionState(panel));

            SetStartState(InventorySelection);
        }
    }
}