using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
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
            foreach (PartySlot partySlot in _party.Slots)
            {
                ConsumableController.OnConsumeItem(_inspectingItem.Consumable, partySlot.HeroBehaviour);
            }
        }

        private void ConsumeOnCharacterIndex(UIItemCharacterPartySlot itemCharacterParty)
        {
            _uiItemCharacterSelection.Hide();
            _uiItemCharacterSelection.Confirmed -= ConsumeOnCharacterIndex;
            ConsumableController.OnConsumeItem(_inspectingItem.Consumable, itemCharacterParty.Hero);
        }
    }
}