using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.Equipment;
using IndiGames.Core.Common;
using UnityEngine;

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

        private int _currentInspectingHeroIndex;

        private int CurrentInspectingHeroIndex
        {
            get => _currentInspectingHeroIndex;
            set
            {
                _currentInspectingHeroIndex = value % Party.Slots.Length;
                if (_currentInspectingHeroIndex < 0) _currentInspectingHeroIndex = Party.Slots.Length - 1;
                var partySlot = Party.Slots[_currentInspectingHeroIndex];
                if (partySlot.IsValid() == false)
                {
                    CurrentInspectingHeroIndex += 1; // might cause a bug here if at 1 go left to 0 and 0 is invalid
                    return;
                }

                StatusPanel.InspectingHero = partySlot.HeroBehaviour;
            }
        }

        public override void OnEnter()
        {
            StatusPanel.ModifyingSlot = ESlot.None;
            StatusPanel.ModifyingCategory = EEquipmentCategory.None;
            
            StatusPanel.Input.MenuCancelEvent += BackToMainMenuNavigation;
            StatusPanel.Input.MenuNavigateEvent += HandleNavigate;
            StatusPanel.ShowMagicStone.EventRaised += ShowMagicStoneMenuRequested;
            UICharacterEquipmentSlot.Pressed += ToEquipmentSelection;
            StatusPanel.InspectingHeroChanged += RenderHero;

            SelectInspectingHero();
        }

        private void SelectInspectingHero()
        {
            if (StatusPanel.InspectingHero != null)
                RenderHero(StatusPanel.InspectingHero);
            else
                CurrentInspectingHeroIndex = 0;
        }

        public override void OnExit()
        {
            StatusPanel.Input.MenuCancelEvent -= BackToMainMenuNavigation;
            StatusPanel.Input.MenuNavigateEvent -= HandleNavigate;
            StatusPanel.ShowMagicStone.EventRaised -= ShowMagicStoneMenuRequested;
            UICharacterEquipmentSlot.Pressed -= ToEquipmentSelection;
            StatusPanel.InspectingHeroChanged -= RenderHero;
        }

        private void BackToMainMenuNavigation() => fsm.RequestStateChange(State.UNFOCUS);

        private void ShowMagicStoneMenuRequested(bool isShow)
        {
            if (!isShow) return;
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            fsm.RequestStateChange(State.MAGIC_STONE);
        }

        private void ToEquipmentSelection(UICharacterEquipmentSlot slot)
        {
            StatusPanel.ModifyingSlot = slot.SlotType;
            StatusPanel.ModifyingCategory = slot.Category;
            fsm.RequestStateChange(State.EQUIPMENT_SELECTION);
        }

        private void RenderHero(HeroBehaviour hero)
        {
            StatusPanel.CharacterEquipmentsPanel.Show(StatusPanel.InspectingHero);
            StatusPanel.CharacterStatsPanel.InspectCharacter(StatusPanel.InspectingHero);
        }

        private void HandleNavigate(Vector2 navigateAxis)
        {
            var direction = (int)navigateAxis.x;
            if (direction == 0) return; // prevent render multiple time
            CurrentInspectingHeroIndex += direction;
        }
    }
}