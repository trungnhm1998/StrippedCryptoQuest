using CryptoQuest.Menus.Status.UI;
using FSM;

namespace CryptoQuest.Menus.Status.States
{
    public static class State
    {
        public const string OVERVIEW = "Overview";
        public const string EQUIPMENT_SELECTION = "EquipmentSelection";
        public const string MAGIC_STONE = "MagicStone";
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
            AddState(State.MAGIC_STONE, new MagicStone(panel));

            SetStartState(State.OVERVIEW);
        }
    }
}