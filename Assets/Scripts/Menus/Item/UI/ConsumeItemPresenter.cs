using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
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
            ActionDispatcher.Dispatch(new ConsumeItemOnParty(_inspectingItem.Consumable.Data));
        }

        private void ConsumeOnCharacterIndex(UIItemCharacterPartySlot itemCharacterParty)
        {
            _uiItemCharacterSelection.Hide();
            _uiItemCharacterSelection.Confirmed -= ConsumeOnCharacterIndex;

            ActionDispatcher.Dispatch(new ConsumeItemOnCharacter(_inspectingItem.Consumable.Data,
                itemCharacterParty.Hero));
        }
    }
}