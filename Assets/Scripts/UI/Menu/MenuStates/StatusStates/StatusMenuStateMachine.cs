using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    /// <summary>
    /// Act as a context for all the sub states in the status menu.
    /// </summary>
    public class StatusMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavStatus = "NavStatus";
        public static readonly string Status = "Status"; // Ghost state, not used.
        public static readonly string Equipment = "Equipment";
        public static readonly string EquipmentSelection = "EquipmentSelection";

        private new readonly UIStatusMenu _panel;

        /// <summary>
        /// Setup the state machine for status menu.
        /// </summary>
        /// <param name="panel"></param>
        public StatusMenuStateMachine(UIStatusMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavStatus, new UnFocusStatusState(panel));
            AddState(Status, new FocusStatusState(panel));
            AddState(Equipment, new EquipmentState(panel));
            AddState(EquipmentSelection, new ChangeEquipmentState(panel));

            SetStartState(Equipment);
        }
    }
}