using CryptoQuest.Menus.Status.UI;
using FSM;

namespace CryptoQuest.Menus.Status.States
{
    /// <summary>
    /// Act as a context for all the sub states in the status menu.
    /// </summary>
    public class StatusMenuStateMachine : StateMachine
    {
        public static readonly string Overview = "Overview";
        public static readonly string EquipmentSelection = "EquipmentSelection";

        /// <summary>
        /// Setup the state machine for status menu.
        /// </summary>
        /// <param name="panel"></param>
        public StatusMenuStateMachine(UIStatusMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            // And problems I means GC problems.
            AddState(Overview, new OverviewHeroStatus(panel));
            AddState(EquipmentSelection, new EquipmentSelection(panel));

            SetStartState(Overview);
        }
    }
}