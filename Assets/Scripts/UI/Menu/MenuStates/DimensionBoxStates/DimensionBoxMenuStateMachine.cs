using System.Diagnostics;
using CryptoQuest.UI.Menu.Panels.DimensionBox;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public class DimensionBoxMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavDimension = "NavDimension";
        public static readonly string DimensionBox = "DimensionBox";

        /// <summary>
        /// Setup the state machine for dimension box menu.
        /// </summary>
        /// <param name="panel"></param>
        public DimensionBoxMenuStateMachine(UIDimensionBoxMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavDimension, new GenericUnfocusState(DimensionBox));
            AddState(DimensionBox, new FocusDimensionBoxState(panel));

            SetStartState(DimensionBox);
        }
    }
}