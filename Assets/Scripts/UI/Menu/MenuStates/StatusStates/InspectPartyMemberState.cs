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
            _characterEquipmentsPanelPanel.Init();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(StatusPanel.TypeSO, true);
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.NavStatus);
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            _characterPanel.ChangeCharacter(direction);
        }
    }
}