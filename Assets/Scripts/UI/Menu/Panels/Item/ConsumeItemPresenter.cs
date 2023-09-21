using CryptoQuest.Gameplay.Inventory;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class ConsumeItemPresenter : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _showHeroSelection;
        [SerializeField] private VoidEventChannelSO _selectAllAliveHeroesEvent;
        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;

        private void OnEnable()
        {
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

        private UIConsumableItem _inspectingItem;

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
            _uiItemCharacterSelection.Confirmed += ConsumeOnCharacterIndex;
            _uiItemCharacterSelection.SelectAllAliveHeroes();
        }

        private void ConsumeOnCharacterIndex(int[] heroIndices)
        {
            _uiItemCharacterSelection.Hide();
            _uiItemCharacterSelection.Confirmed -= ConsumeOnCharacterIndex;
            ConsumableController.OnConsumeItem(_inspectingItem.Consumable, heroIndices);
        }
    }
}