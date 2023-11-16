using CryptoQuest.Core;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.UI.Tooltips;

namespace CryptoQuest.Menus.Status.States
{
    public class EquipmentSelection : StatusStateBase
    {
        public EquipmentSelection(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToOverview;
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToOverview;

            ActionDispatcher.Dispatch(new HideEquipmentTooltip());
            StatusPanel.EquipmentsInventoryPanel.Hide();
        }

        private void BackToOverview()
        {
            fsm.RequestStateChange(StatusMenuStateMachine.Overview);
        }
    }
}