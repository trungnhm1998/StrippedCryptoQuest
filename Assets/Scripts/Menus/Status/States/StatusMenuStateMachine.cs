using CryptoQuest.Menus.Status.UI;
using FSM;

namespace CryptoQuest.Menus.Status.States
{
    public static class State
    {
        public const string OVERVIEW = "Overview";
        public const string EQUIPMENT_SELECTION = "EquipmentSelection";
        public const string MAGIC_STONE_SLOT_SELECTION = "SlotSelection";
        public const string MAGIC_STONE_ELEMENT_NAVIGATION = "ElementNavigation";
        public const string MAGIC_STONE_SELECTION = "StoneSelection";
        public const string UNFOCUS = "Unfocus";
    }

    /// <summary>
    /// Act as a context for all the sub states in the status menu.
    /// </summary>
    public class StatusMenuStateMachine : StateMachine
    {
        public StatusMenuStateMachine(UIStatusMenu panel) : base(panel)
        {
            AddState(State.UNFOCUS, new Unfocus(panel));
            AddState(State.OVERVIEW, new OverviewHeroStatus(panel));
            AddState(State.EQUIPMENT_SELECTION, new ModifyEquipments(panel));
            AddState(State.MAGIC_STONE_SLOT_SELECTION, new MagicStone.SlotSelection(panel));
            AddState(State.MAGIC_STONE_ELEMENT_NAVIGATION, new MagicStone.ElementNavigation(panel));
            AddState(State.MAGIC_STONE_SELECTION, new MagicStone.StoneSelection(panel));

            SetStartState(State.OVERVIEW);
        }
    }
}