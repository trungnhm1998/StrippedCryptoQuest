using CryptoQuest.Menus.Status.UI;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States
{
    public class MagicStone : StatusStateBase
    {
        public MagicStone(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Input.MenuCancelEvent += BackToEquipmentSelection;
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToEquipmentSelection;
        }

        private void BackToEquipmentSelection()
        {
            StatusPanel.ShowMagicStone.RaiseEvent(false);
            fsm.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }
    }
}