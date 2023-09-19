using System.Diagnostics;
using CryptoQuest.UI.Menu.Panels.DimensionBox;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public class DimensionBoxMenuStateMachine : MenuStateMachine
    {
        public static readonly string NavDimension = "NavDimension";
        public static readonly string TransferTypeSelection = "TransferTypeSelection";
        public static readonly string EquipmentTransfer = "EquipmentTransfer";
        public static readonly string MetadTransfer = "MetadTransfer";

        /// <summary>
        /// Setup the state machine for dimension box menu.
        /// </summary>
        /// <param name="panel"></param>
        public DimensionBoxMenuStateMachine(UIDimensionBoxMenu panel) : base(panel)
        {
            // Could create a factory here if new keyword becomes a problem.
            AddState(NavDimension, new GenericUnfocusState(TransferTypeSelection));
            AddState(TransferTypeSelection, new TransferTypeSelectionState(panel));
            AddState(EquipmentTransfer, new EquipmentTransferState(panel));

            SetStartState(TransferTypeSelection);
        }
    }
}