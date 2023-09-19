using System;
using CryptoQuest.UI.Menu.Panels.DimensionBox;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public class TransferTypeSelectionState : DimensionBoxStateBase
    {
        public TransferTypeSelectionState(UIDimensionBoxMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO);
            DimensionBoxPanel.DimensionBoxTypeSelection.SetButtonsActive(true);
            DimensionBoxPanel.DimensionBoxTypeSelection.SetDefaultSelection();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(DimensionBoxMenuStateMachine.NavDimension);
        }

        public override void OnExit()
        {
            base.OnExit();
            DimensionBoxPanel.DimensionBoxTypeSelection.SetButtonsActive(false);
        }
    }
}
