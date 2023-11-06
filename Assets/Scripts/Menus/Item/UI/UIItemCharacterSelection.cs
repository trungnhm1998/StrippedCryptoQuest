using System;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menus.Item.States;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIItemCharacterSelection : MonoBehaviour
    {
        public event Action<UICharacterPartySlot> Confirmed;
        [SerializeField] private UICharacterPartySlot[] _partySlots;

        /// <summary>
        /// Actively update data on reopening
        /// </summary>
        private void OnEnable()
        {
            ItemConsumeState.Cancelled += Hide;
            var party = ServiceProvider.GetService<IPartyController>();
            for (var index = 0; index < party.Slots.Length; index++)
            {
                var slot = party.Slots[index];
                var ui = _partySlots[index];
                ui.gameObject.SetActive(slot.IsValid());
                if (!slot.IsValid()) continue;
                ui.Init(slot.HeroBehaviour, index);
                // ui.SetSelectedCallback(OnHeroSelected);
            }
        }

        private void OnDisable()
        {
            ItemConsumeState.Cancelled -= Hide;
        }

        /// <summary>
        /// When a button got pressed, but if we open to select all heroes
        ///
        /// use the first button to select all heroes
        /// </summary>
        /// <param name="hero"></param>
        private void OnHeroSelected(UICharacterPartySlot hero)
        {
            _selectingAll = false;
            Confirmed?.Invoke(hero);
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