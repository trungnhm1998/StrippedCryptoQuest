using CryptoQuest.Menus.Status.UI;
using FSM;

namespace CryptoQuest.Menus.Status.States
{
    /// <summary>
    /// Act as a context for all the sub states in the status menu.
    /// </summary>
    public class StatusMenuStateMachine : StateMachine
    {
        public static readonly string Status = "Status"; // Ghost state, not used.
        public static readonly string Equipment = "Equipment";
        public static readonly string EquipmentSelection = "EquipmentSelection";

        /// <summary>
        /// Setup the state machine for status menu.
        /// </summary>
        /// <param name="panel"></param>
        public StatusMenuStateMachine(UIStatusMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            // And problems I means GC problems.
            AddState(Status, new FocusStatusState(panel));
            AddState(Equipment, new InspectPartyMemberState(panel));
            AddState(EquipmentSelection, new ChangeEquipmentState(panel));

            SetStartState(Equipment);
        }
    }
}