using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.System;
using CryptoQuest.UI.Menu;
using IndiGames.Core.Common;
using UnityEngine;
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

        private int _currentPartySlot = 0;
        private bool _isBackToNavigation;

        public override void OnEnter()
        {
            _isBackToNavigation = false;

            UIMainMenu.BackToNavigation += HandleBackToNavigation;
            StatusPanel.Focusing += FocusSelectEquipmentPanel;
            StatusPanel.Input.MenuCancelEvent += HandleCancel;
            StatusPanel.Input.MenuNavigateEvent += HandleNavigate;
            StatusPanel.ShowMagicStone.EventRaised += ShowMagicStoneMenuRequested;

            RegisterEquipmentButtonsEvent();
            FocusSelectEquipmentPanel();

            RenderHero(_currentPartySlot);
        }

        private void ShowMagicStoneMenuRequested(bool isShow)
        {
            if (!isShow) return;
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            fsm.RequestStateChange(StatusMenuStateMachine.MagicStone);
        }

        private void HandleBackToNavigation()
        {
            _isBackToNavigation = true;
        }

        public override void OnExit()
        {
            UIMainMenu.BackToNavigation -= HandleBackToNavigation;
            StatusPanel.Focusing -= FocusSelectEquipmentPanel;
            StatusPanel.Input.MenuCancelEvent -= HandleCancel;
            StatusPanel.Input.MenuNavigateEvent -= HandleNavigate;
            UnregisterEquipmentButtonsEvent();
        }

        private void FocusSelectEquipmentPanel()
        {
            _isBackToNavigation = false;
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

        private void RenderHero(int partySlot)
        {
            StatusPanel.InspectingHero = Party.Slots[partySlot].HeroBehaviour;
            StatusPanel.CharacterEquipmentsPanel.Show(StatusPanel.InspectingHero);
            StatusPanel.CharacterStatsPanel.InspectCharacter(StatusPanel.InspectingHero);
        }

        private void HandleNavigate(Vector2 navigateAxis)
        {
            if (_isBackToNavigation) return;

            // render hero outside will causes re-rendering the hero when navigate y axis
            if (navigateAxis.x > 0)
            {
                _currentPartySlot++;
                if (_currentPartySlot >= Party.Slots.Length)
                    _currentPartySlot = 0;

                RenderHero(_currentPartySlot);
            }
            else if (navigateAxis.x < 0)
            {
                _currentPartySlot--;
                if (_currentPartySlot < 0)
                    _currentPartySlot = Party.Slots.Length - 1;

                RenderHero(_currentPartySlot);
            }
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }
    }
}