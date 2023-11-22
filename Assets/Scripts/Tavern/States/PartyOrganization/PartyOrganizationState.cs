using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Tavern.UI;
using TinyMessenger;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;
using Random = UnityEngine.Random;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class PartyOrganizationState : StateMachineBehaviourBase
    {
        [SerializeField] private PartySO _party;

        private TavernController _controller;
        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;

        private List<Obj.Character> _cachedInPartyCharactersData = new List<Obj.Character>();
        private List<Obj.Character> _cachedNonPartyCharactersData = new List<Obj.Character>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Party Organization");

        private bool _hasGotHeroesFromServer = false;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UIPartyOrganization.Contents.SetActive(true);
            _controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Clear();
            _controller.UIPartyOrganization.SelectedPartyCharacterIds.Clear();
            _controller.UIPartyOrganization.HandleListInteractable();
            UITavernItem.Pressed += _controller.UIPartyOrganization.Transfer;

            _getGameDataSucceedEvent =
                ActionDispatcher.Bind<GetFilteredInGameNftCharactersSucceed>(GetInGameCharacters);

            if (!_hasGotHeroesFromServer)
            {
                _hasGotHeroesFromServer = true;
                ActionDispatcher.Dispatch(new GetInGameHeroes());
            }

            _controller.MerchantInputManager.CancelEvent += CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
        }

        protected override void OnExit()
        {
            UITavernItem.Pressed -= _controller.UIPartyOrganization.Transfer;

            ActionDispatcher.Unbind(_getGameDataSucceedEvent);

            _controller.MerchantInputManager.CancelEvent -= CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent -= ResetTransferRequested;            
        }

        private void GetInPartyCharacters()
        {
            _cachedInPartyCharactersData.Clear();
            foreach (var partySlot in _party.GetParty())
            {
                if (partySlot.IsValid() == false) continue;
                var isMain = partySlot.Hero.Id == 0;
                if (isMain) continue;
                _cachedInPartyCharactersData.Add(new Obj.Character()
                {
                    id = partySlot.Hero.Id,
                    name = partySlot.Hero.Origin.DetailInformation.LocalizedName.GetLocalizedString(),
                    level = Random.Range(0, 100)
                });
            }

            _controller.UIParty.SetData(_cachedInPartyCharactersData);
        }

        private void GetInGameCharacters(GetFilteredInGameNftCharactersSucceed obj)
        {
            GetInPartyCharacters();
            _cachedNonPartyCharactersData = obj.FilteredInGameCharacters;
            if (obj.FilteredInGameCharacters.Count <= 0) return;
            _controller.UINonParty.SetData(obj.FilteredInGameCharacters);
        }

        private void CancelPartyOrganization()
        {
            _hasGotHeroesFromServer = false;
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

            _controller.UIParty.SetData(_cachedInPartyCharactersData);
            _controller.UINonParty.SetData(_cachedNonPartyCharactersData);
        }
    }
}