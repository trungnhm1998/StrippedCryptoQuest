using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using CryptoQuest.Tavern.UI;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class PartyOrganizationState : StateMachineBehaviourBase
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private HeroInventorySO _heroInventory;

        private TavernController _controller;
        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;

        private List<HeroSpec> _cachedInPartyHeroes = new List<HeroSpec>();
        private List<HeroSpec> _cachedNonPartyHeroes = new List<HeroSpec>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Party Organization");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UIPartyOrganization.Contents.SetActive(true);
            _controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Clear();
            _controller.UIPartyOrganization.SelectedPartyCharacterIds.Clear();

            GetInPartyHeroes();
            GetNonPartyHeroes();
            _controller.UIPartyOrganization.HandleListInteractable();
            UITavernItem.Pressed += _controller.UIPartyOrganization.Transfer;

            _controller.MerchantInputManager.CancelEvent += CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent += ViewCharacterDetails;
        }

        protected override void OnExit()
        {
            UITavernItem.Pressed -= _controller.UIPartyOrganization.Transfer;

            _controller.MerchantInputManager.CancelEvent -= CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent -= ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent -= ViewCharacterDetails;
        }

        private void GetInPartyHeroes()
        {
            _cachedInPartyHeroes.Clear();
            foreach (var partySlot in _party.GetParty())
            {
                if (partySlot.IsValid() == false) continue;
                var isMain = partySlot.Hero.Origin.DetailInformation.Id == 0;
                if (isMain) continue;
                _cachedInPartyHeroes.Add(partySlot.Hero);
            }

            _controller.UIParty.SetData(_cachedInPartyHeroes);
        }

        private void GetNonPartyHeroes()
        {
            if (_heroInventory.OwnedHeroes.Count <= 0) return;
            _cachedNonPartyHeroes = _heroInventory.OwnedHeroes;
            for (var index = _heroInventory.OwnedHeroes.Count - 1; index >= 0; index--)
            {
                var nonPartyHero = _heroInventory.OwnedHeroes[index];
                foreach (var inPartyHero in _cachedInPartyHeroes)
                {
                    if (nonPartyHero.Id != inPartyHero.Id) continue;
                    _cachedNonPartyHeroes.Remove(nonPartyHero);
                }
            }

            _controller.UINonParty.SetData(_cachedNonPartyHeroes);
        }

        private void CancelPartyOrganization()
        {
            _controller.UIPartyOrganization.Contents.SetActive(false);
            StateMachine.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UIPartyOrganization.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            if (_controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Count == 0 &&
                _controller.UIPartyOrganization.SelectedPartyCharacterIds.Count == 0) return;

            StateMachine.Play(ConfirmState);
        }

        private void ResetTransferRequested()
        {
            if (_controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Count == 0 &&
                _controller.UIPartyOrganization.SelectedPartyCharacterIds.Count == 0) return;

            _controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Clear();
            _controller.UIPartyOrganization.SelectedPartyCharacterIds.Clear();

            _controller.UIParty.SetData(_cachedInPartyHeroes);
            _controller.UINonParty.SetData(_cachedNonPartyHeroes);
            _controller.UIParty.UpdateList();
            _controller.UINonParty.UpdateList();
        }

        private void ViewCharacterDetails()
        {
            var currentItem = EventSystem.current.currentSelectedGameObject.GetComponent<UITavernItem>();
            currentItem.OnInspectDetails();
            _controller.SpecInitializer.Init(currentItem.Hero);
        }
    }
}