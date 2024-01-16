using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Merchant;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharactersParty : MonoBehaviour
    {
        [SerializeField] private Animator _stateMachine;
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
            foreach (var uiCharacterInfo in _characterInfos) uiCharacterInfo.Init(new HeroSpec());

            var uiIndex = 0;
            for (var i = 0; i < _partySO.Count; i++)
            {
                if (_partySO[i].Hero.Id == 0) continue;
                _characterInfos[uiIndex++].Init(_partySO[i].Hero);
            }
        }

        private void RemoveHeroFromParty(UICharacterInfo uiSlot)
        {
            var selectedHero = uiSlot.Spec;
            if (selectedHero.IsValid() == false)
            {
                AddingHeroToParty(uiSlot);
                return;
            }

            uiSlot.Init(new HeroSpec());
            List<PartySlotSpec> newParty = new List<PartySlotSpec>();
            for (var index = 0; index < _partySO.Count; index++)
            {
                var partySlot = _partySO[index];
                if (partySlot.Hero.Id == selectedHero.Id) continue;
                newParty.Add(partySlot);
            }

            _partySO.SetParty(newParty.ToArray());
            _uiCharacterInventoryList.Refresh();
        }

        private void AddingHeroToParty(UICharacterInfo uiSlot)
        {
            _interactingSlot = uiSlot;
            _interactingSlot.MarkAsInteracting();
            _uiCharacterInventoryList.GetComponentInChildren<Selectable>().Select();
            _uiCharacterInventoryList.ItemSelected += AddHeroToParty;
            _input.CancelEvent += CancelAddHeroToParty;
            _stateMachine.Play("AddingHeroIntoParty");
        }

        /// <summary>
        /// Add to the last slot in the party
        /// </summary>
        private void AddHeroToParty(UICharacterListItem uiListItem)
        {
            _uiCharacterInventoryList.ItemSelected -= AddHeroToParty;
            _input.CancelEvent -= CancelAddHeroToParty;
            var heroSpec = uiListItem.Spec;
            _interactingSlot.Init(heroSpec);
            FocusLastInteractedSlot();
            PartySlotSpec[] newParty = new PartySlotSpec[_partySO.Count + 1];
            _partySO.GetParty().CopyTo(newParty, 0);
            newParty[^1] = new PartySlotSpec {Hero = heroSpec};
            _partySO.SetParty(newParty);
        }

        private void CancelAddHeroToParty()
        {
            _input.CancelEvent -= CancelAddHeroToParty;
            FocusLastInteractedSlot();
        }

        private void FocusLastInteractedSlot()
        {
            _stateMachine.Play("OrganizeParty");
            EventSystem.current.SetSelectedGameObject(_interactingSlot.gameObject);
            _interactingSlot.MarkAsInteracting(false);
            _interactingSlot = null;
        }
    }
}