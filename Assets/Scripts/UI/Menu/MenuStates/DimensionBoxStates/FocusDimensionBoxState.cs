using CryptoQuest.UI.Menu.Panels.DimensionBox;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public class FocusDimensionBoxState : DimensionBoxStateBase
    {
        public FocusDimensionBoxState(UIDimensionBoxMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            DimensionBoxPanel.DimensionBoxTabHeader.Init();
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(DimensionBoxPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(DimensionBoxMenuStateMachine.NavDimension);
        }

        public override void Interact()
        {
            base.Interact();
            MenuStateMachine.RequestStateChange(DimensionBoxMenuStateMachine.DimensionBox);
        }
    }
}
