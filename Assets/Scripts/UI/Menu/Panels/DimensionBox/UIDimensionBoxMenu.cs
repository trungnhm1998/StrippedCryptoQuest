using CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIDimensionBoxMenu : UIMenuPanel
    {
        [field: SerializeField] public UIDimensionBoxTransferType DimensionBoxTabHeader { get; private set; }
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new DimensionBoxMenuStateMachine(this);
        }
    }
}
