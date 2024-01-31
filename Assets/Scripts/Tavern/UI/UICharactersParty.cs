using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Merchant;
using CryptoQuest.System.SaveSystem.Savers;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharactersParty : MonoBehaviour
    {
        public event Action Closed;

        [SerializeField] private MerchantInput _input;
        [SerializeField] private PartySO _partySO;
        [SerializeField] private UICharacterInfo[] _characterInfos;
        [SerializeField] private UICharacterInventoryList _uiCharacterInventoryList;

        private UICharacterInfo _interactingSlot;

        private void Start()
        {
            foreach (var uiCharacterInfo in _characterInfos)
            {
                var button = uiCharacterInfo.GetComponent<Button>();
                button.onClick.AddListener(() => RemoveHeroFromParty(uiCharacterInfo));
            }
        }

        private void OnEnable()
        {
            _input.CancelEvent += OnClose;
            foreach (var uiCharacterInfo in _characterInfos) uiCharacterInfo.Init(new HeroSpec());

            var uiIndex = 0;
            for (var i = 0; i < _partySO.Count; i++)
            {
                if (_partySO[i].Hero.Origin.DetailInformation.Id == 0) continue;
                _characterInfos[uiIndex++].Init(_partySO[i].Hero);
            }
        }

        private void OnDisable()
        {
            _input.CancelEvent -= OnClose;
            _input.CancelEvent -= CancelAddHeroToParty;
        }

        private void OnClose()
        {
            _input.CancelEvent -= OnClose;
            Closed?.Invoke();
        }

        private void RemoveHeroFromParty(UICharacterInfo uiSlot)
        {
            var selectedHero = uiSlot.Spec;
            var isSlotEmpty = selectedHero.IsValid() == false;
            if (isSlotEmpty && !_uiCharacterInventoryList.HasHeroToRecruit())
            {
                Debug.Log("Use transfer to add more heroes");
                return;
            }

            if (isSlotEmpty)
            {
                AddingHeroToParty(uiSlot);
                return;
            }

            uiSlot.Init(new HeroSpec());
            List<PartySlotSpec> newParty = new List<PartySlotSpec>();
            for (var index = 0; index < _partySO.Count; index++)
            {
                var partySlot = _partySO[index];
                if (partySlot.Hero.Id == selectedHero.Id)
                {
                    foreach (var equipmentSlot in partySlot.EquippingItems.Slots)
                    {
                        if (equipmentSlot.IsValid() == false) continue;
                        var equipment = equipmentSlot.Equipment;
                        equipment.AttachCharacterId = -1;
                    }
                    continue;
                }
                newParty.Add(partySlot);
            }

            _partySO.SetParty(newParty.ToArray());
            ActionDispatcher.Dispatch(new SaveEquipmentAction());
            _uiCharacterInventoryList.Refresh();
        }

        private void AddingHeroToParty(UICharacterInfo uiSlot)
        {
            _input.CancelEvent -= OnClose;
            _input.CancelEvent += CancelAddHeroToParty;
            _interactingSlot = uiSlot;
            _interactingSlot.MarkAsInteracting();
            _uiCharacterInventoryList.GetComponentInChildren<Selectable>().Select();
            _uiCharacterInventoryList.ItemSelected += AddHeroToParty;
        }

        /// <summary>
        /// Add to the last slot in the party
        /// </summary>
        private void AddHeroToParty(UICharacterListItem uiListItem)
        {
            _input.CancelEvent += OnClose;
            _uiCharacterInventoryList.ItemSelected -= AddHeroToParty;
            _input.CancelEvent -= CancelAddHeroToParty;
            var heroSpec = uiListItem.Spec;
            _interactingSlot.Init(heroSpec);
            FocusLastInteractedSlot();
            PartySlotSpec[] newParty = new PartySlotSpec[_partySO.Count + 1];
            _partySO.GetParty().CopyTo(newParty, 0);
            newParty[^1] = new PartySlotSpec { Hero = heroSpec };
            _partySO.SetParty(newParty);
        }

        private void CancelAddHeroToParty()
        {
            _input.CancelEvent -= CancelAddHeroToParty;
            _input.CancelEvent += OnClose;
            FocusLastInteractedSlot();
        }

        private void FocusLastInteractedSlot()
        {
            EventSystem.current.SetSelectedGameObject(_interactingSlot.gameObject);
            _interactingSlot.MarkAsInteracting(false);
            _interactingSlot = null;
        }
    }
}