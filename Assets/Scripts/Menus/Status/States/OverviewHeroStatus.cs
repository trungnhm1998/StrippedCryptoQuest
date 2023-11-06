using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.System;
using CryptoQuest.UI.Menu;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.States
{
    /// <summary>
    /// This is the state for <see cref="StatusMenuStateMachine"/> that also defined to be a default state when
    /// enter the State Machine.
    /// </summary>
    public class OverviewHeroStatus : StatusStateBase
    {
        private IPartyController _party;
        private IPartyController Party => _party ??= ServiceProvider.GetService<IPartyController>();
        public OverviewHeroStatus(UIStatusMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            StatusPanel.Focusing += FocusSelectEquipmentPanel;
            StatusPanel.Input.MenuCancelEvent += HandleCancel;
            RegisterEquipmentButtonsEvent();
            FocusSelectEquipmentPanel();

            // Render first hero infos in the party
            StatusPanel.InspectingHero ??= Party.Slots[0].HeroBehaviour;
            StatusPanel.CharacterEquipmentsPanel.Show(StatusPanel.InspectingHero);
            StatusPanel.CharacterStatsPanelPanel.InspectCharacter(StatusPanel.InspectingHero);
        }

        public override void OnExit()
        {
            StatusPanel.Focusing -= FocusSelectEquipmentPanel;
            StatusPanel.Input.MenuCancelEvent -= HandleCancel;
            UnregisterEquipmentButtonsEvent();
        }

        private void FocusSelectEquipmentPanel()
        {
            StatusPanel.CharacterEquipmentsPanel.SelectDefault();
        }

        private void RegisterEquipmentButtonsEvent()
        {
            foreach (var slot in StatusPanel.CharacterEquipmentsPanel.EquipmentSlots)
                slot.Value.ShowEquipmentsInventoryWithType += ToEquipmentSelection;
        }

        private void UnregisterEquipmentButtonsEvent()
        {
            foreach (var slot in StatusPanel.CharacterEquipmentsPanel.EquipmentSlots)
                slot.Value.ShowEquipmentsInventoryWithType -= ToEquipmentSelection;
        }

        private MultiInputButton _lastSelectedEquipmentSlot;

        private void ToEquipmentSelection(EquipmentSlot.EType slotType, EEquipmentCategory categoryType)
        {
            _lastSelectedEquipmentSlot = EventSystem.current.currentSelectedGameObject.GetComponent<MultiInputButton>();
            StatusPanel.CharacterEquipmentsPanel.Hide();
            StatusPanel.EquipmentsInventoryPanel.RenderEquipmentsInInventory(StatusPanel.InspectingHero, slotType,
                categoryType);
            fsm.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }
    }
}