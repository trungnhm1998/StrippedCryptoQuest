using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class EquipmentState : StatusStateBase
    {
        private UIEquipmentOverview _equipmentOverviewPanel;
        private UIStatusCharacter _characterPanel;

        public EquipmentState(UIStatusMenu statusPanel) : base(statusPanel)
        {
            _equipmentOverviewPanel = statusPanel.EquipmentOverviewPanel;
            _characterPanel = statusPanel.CharacterPanel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO);
            _equipmentOverviewPanel.EquipmentSlotSelected += ChangeEquipment;
            _equipmentOverviewPanel.Init();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
            _equipmentOverviewPanel.DeInit();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.NavStatus);
        }

        public override void OnExit()
        {
            base.OnExit();
            _equipmentOverviewPanel.DeInit();
            _equipmentOverviewPanel.EquipmentSlotSelected -= ChangeEquipment;
        }

        private void ChangeEquipment(EEquipmentCategory type)
        {
            StatusPanel.EquippingType = type;
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            _characterPanel.ChangeCharacter(direction);
        }
    }
}