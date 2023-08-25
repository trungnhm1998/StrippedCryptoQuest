using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class InspectPartyMemberState : StatusStateBase
    {
        private UICharacterEquipmentsPanel _characterEquipmentsPanelPanel;
        private UIStatusCharacter _characterPanel;

        public InspectPartyMemberState(UIStatusMenu statusPanel) : base(statusPanel)
        {
            _characterEquipmentsPanelPanel = statusPanel.CharacterEquipmentsPanel;
            _characterPanel = statusPanel.CharacterPanel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO);
            _characterEquipmentsPanelPanel.EquipmentSlotSelected += ChangeEquipment;
            _characterEquipmentsPanelPanel.Init();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
            _characterEquipmentsPanelPanel.DeInit();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.NavStatus);
        }

        public override void OnExit()
        {
            base.OnExit();
            _characterEquipmentsPanelPanel.DeInit();
            _characterEquipmentsPanelPanel.EquipmentSlotSelected -= ChangeEquipment;
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