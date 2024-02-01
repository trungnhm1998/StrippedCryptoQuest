using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI;
using CryptoQuest.Menus.Status.UI.Equipment;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
                var partySize = Party.PartySO.Count;
                _currentInspectingHeroIndex = value % partySize;
                if (_currentInspectingHeroIndex < 0) _currentInspectingHeroIndex = partySize - 1;
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
            StatusPanel.Input.TabChangeEvent += HandleNavigate;
            UICharacterEquipmentSlot.Pressed += ToEquipmentSelection;
            StatusPanel.InspectingHeroChanged += RenderHero;

            StatusPanel.EquipmentPreviewer.ResetPreviewer();

            SelectInspectingHero();
            var selectable = StatusPanel.CharacterEquipmentsPanel.GetComponentInChildren<Selectable>();
            EventSystem.current.SetSelectedGameObject(selectable.gameObject);
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
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            StatusPanel.Input.MenuCancelEvent -= BackToMainMenuNavigation;
            StatusPanel.Input.TabChangeEvent -= HandleNavigate;
            UICharacterEquipmentSlot.Pressed -= ToEquipmentSelection;
            StatusPanel.InspectingHeroChanged -= RenderHero;
        }

        private void BackToMainMenuNavigation() => fsm.RequestStateChange(State.UNFOCUS);

        private void ToEquipmentSelection(UICharacterEquipmentSlot slot)
        {
            StatusPanel.ModifyingSlot = slot.SlotType;
            StatusPanel.ModifyingCategory = slot.Category;
            fsm.RequestStateChange(State.EQUIPMENT_SELECTION);
        }

        private void RenderHero(HeroBehaviour hero)
        {
            StatusPanel.ShowTooltipEvent.RaiseEvent(false);
            StatusPanel.CharacterEquipmentsPanel.Show(StatusPanel.InspectingHero);
            StatusPanel.CharacterStatsPanel.InspectCharacter(StatusPanel.InspectingHero);
        }

        private void HandleNavigate(float direction)
        {
            if (direction == 0) return; // prevent render multiple time
            CurrentInspectingHeroIndex += (int)direction;
        }
    }
}