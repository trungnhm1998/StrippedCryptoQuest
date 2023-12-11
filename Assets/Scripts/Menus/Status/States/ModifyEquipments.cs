using CryptoQuest.Menus.Status.UI;
using UnityEngine;

namespace CryptoQuest.Menus.Status.States
{
    public class ModifyEquipments : StatusStateBase
    {
        public ModifyEquipments(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            StatusPanel.Input.MenuCancelEvent += BackToOverview;

            StatusPanel.CharacterEquipmentsPanel.Hide();
            StatusPanel.EquipmentsInventoryPanel.SetActiveAllEquipmentButtons(true);
            StatusPanel.EquipmentsInventoryPanel.RenderEquipmentsInInventory(StatusPanel.InspectingHero,
                StatusPanel.ModifyingSlot, StatusPanel.ModifyingCategory);
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToOverview;
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            StatusPanel.EquipmentsInventoryPanel.SetActiveAllEquipmentButtons(false);
        }

        private void BackToOverview()
        {
            StatusPanel.EquipmentsInventoryPanel.Hide();
            fsm.RequestStateChange(State.OVERVIEW);
        }
    }
}