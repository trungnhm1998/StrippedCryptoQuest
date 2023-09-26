using System;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIItemCharacterSelection : MonoBehaviour
    {
        public event Action<int[]> Confirmed;
        [SerializeField] private UICharacterPartySlot[] _partySlots;

        /// <summary>
        /// Actively update data on reopening
        /// </summary>
        private void OnEnable()
        {
            ConsumingItemState.Cancelled += Hide;
            var party = ServiceProvider.GetService<IPartyController>();
            for (var index = 0; index < party.Slots.Length; index++)
            {
                var slot = party.Slots[index];
                var ui = _partySlots[index];
                ui.gameObject.SetActive(slot.IsValid());
                if (!slot.IsValid()) continue;
                ui.Init(slot.HeroBehaviour, index);
                ui.SetSelectedCallback(OnHeroSelected);
            }
        }

        private void OnDisable()
        {
            ConsumingItemState.Cancelled -= Hide;
        }

        /// <summary>
        /// When a button got pressed, but if we open to select all heroes
        ///
        /// use the first button to select all heroes
        /// </summary>
        /// <param name="index"></param>
        private void OnHeroSelected(int index)
        {
            var indices = _selectingAll ? new int[] { 0, 1, 2, 3 } : new int[] { index };
            _selectingAll = false;
            Confirmed?.Invoke(indices);
        }

        public void Hide()
        {
            EnableAllButtons(false);
        }

        /// <summary>
        /// Add buttons to unity event system
        /// </summary>
        private void EnableAllButtons(bool enable = true)
        {
            foreach (var slot in _partySlots)
                slot.Interactable = enable;
        }

        public void SelectHero()
        {
            EnableAllButtons();
            EventSystem.current.SetSelectedGameObject(_partySlots[0].GetComponentInChildren<Selectable>().gameObject);
        }

        private bool _selectingAll;

        public void SelectAllAliveHeroes()
        {
            SelectHero();
            _selectingAll = true;

            foreach (var slot in _partySlots)
                slot.Select();
        }
    }
}