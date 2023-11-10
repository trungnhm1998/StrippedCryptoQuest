using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Menus.Item.UI
{
    public class ConsumeItemPresenter : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _showHeroSelection;
        [SerializeField] private VoidEventChannelSO _selectAllAliveHeroesEvent;
        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;

        private IPartyController _party;
        private UIConsumableItem _inspectingItem;

        private void OnEnable()
        {
            _party = ServiceProvider.GetService<IPartyController>();

            _showHeroSelection.EventRaised += ShowSelectSingleHeroUI;
            _selectAllAliveHeroesEvent.EventRaised += ShowSelectAllAliveHeroes;
            UIConsumableItem.Inspecting += SaveLastInspectingItem;
        }

        /// <summary>
        /// I don't want to show this panel when the main menu is disabled
        /// </summary>
        private void OnDisable()
        {
            _showHeroSelection.EventRaised -= ShowSelectSingleHeroUI;
            _selectAllAliveHeroesEvent.EventRaised -= ShowSelectAllAliveHeroes;
            UIConsumableItem.Inspecting -= SaveLastInspectingItem;
        }

        private void SaveLastInspectingItem(UIConsumableItem item)
        {
            _inspectingItem = item;
        }

        private void ShowSelectSingleHeroUI()
        {
            _uiItemCharacterSelection.Confirmed += ConsumeOnCharacterIndex;
            _uiItemCharacterSelection.SelectHero();
        }

        private void ShowSelectAllAliveHeroes()
        {
            ConsumableController.OnConsumeItem(_inspectingItem.Consumable, _party.Slots
                .Where(slot => slot.IsValid())
                .Select(slot => slot.HeroBehaviour)
                .ToList());
        }

        private void ConsumeOnCharacterIndex(UIItemCharacterPartySlot itemCharacterParty)
        {
            _uiItemCharacterSelection.Hide();
            _uiItemCharacterSelection.Confirmed -= ConsumeOnCharacterIndex;
            
            ConsumableController.OnConsumeItem(_inspectingItem.Consumable,
                new List<HeroBehaviour> { itemCharacterParty.Hero });
        }
    }
}