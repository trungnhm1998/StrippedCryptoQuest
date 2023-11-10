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
        public event Action<UIItemCharacterPartySlot> Confirmed;
        [SerializeField] private UIItemCharacterPartySlot[] _partySlots;
        private IPartyController _party;

        /// <summary>
        /// Actively update data on reopening
        /// </summary>
        private void OnEnable()
        {
            ItemConsumeState.Cancelled += Hide;
            _party = ServiceProvider.GetService<IPartyController>();
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var slot = _party.Slots[index];
                var ui = _partySlots[index];
                ui.gameObject.SetActive(slot.IsValid());
                if (!slot.IsValid()) continue;
                ui.Init(slot.HeroBehaviour, index);
                ui.Selected += OnHeroSelected;
            }
        }

        private void OnDisable()
        {
            ItemConsumeState.Cancelled -= Hide;
            for (var index = 0; index < _party.Slots.Length; index++)
            {
                var ui = _partySlots[index];
                ui.Selected -= OnHeroSelected;
            }
        }

        /// <summary>
        /// When a button got pressed, but if we open to select all heroes
        ///
        /// use the first button to select all heroes
        /// </summary>
        /// <param name="hero"></param>
        private void OnHeroSelected(UIItemCharacterPartySlot hero)
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