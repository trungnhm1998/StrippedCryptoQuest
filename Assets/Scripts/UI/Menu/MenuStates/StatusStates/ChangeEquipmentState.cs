﻿using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class ChangeEquipmentState : StatusStateBase
    {
        private readonly UIEquipmentList _equipmentListPanel;

        public ChangeEquipmentState(UIStatusMenu statusPanel) : base(statusPanel)
        {
            _equipmentListPanel = statusPanel.EquipmentListPanel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _equipmentListPanel.Show(StatusPanel.EquippingType);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.Equipment);
        }

        public override void OnExit()
        {
            base.OnExit();
            _equipmentListPanel.Hide();
        }
    }
}