using CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIDimensionBoxMenu : UIMenuPanel
    {
        [field: SerializeField] public UITransferTypeSelection DimensionBoxTypeSelection { get; private set; }
        [field: SerializeField] public UIEquipmentSection EquipmentTransferSection { get; private set; }
        [field: SerializeField] public UIMetadSection MetadTransferSection { get; private set; }


        private void Awake()
        {
            DimensionBoxTypeSelection.MainPanel = this;
        }

        private DimensionBoxMenuStateMachine _fsm;
        public DimensionBoxMenuStateMachine Fsm => _fsm;

        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            if (_fsm == null)
                _fsm = new DimensionBoxMenuStateMachine(this);
            return _fsm;
        }
    }
}
