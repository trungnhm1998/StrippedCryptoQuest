using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.Tavern.States.CharacterReplacement;
using CryptoQuest.Tavern.UI;
using CryptoQuest.Tavern.UI.CharacterReplacement;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class PartyOrganizationState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getInPartyNftCharacters;
        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;

        private List<ICharacterData> _cachedInPartyCharactersData = new List<ICharacterData>();
        private List<ICharacterData> _cachedNonPartyCharactersData = new List<ICharacterData>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Party Organization");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UIPartyOrganization.gameObject.SetActive(true);
            _controller.UIPartyOrganization.Contents.SetActive(true);
            UITavernItem.Pressed += _controller.UIPartyOrganization.Transfer;
            UICharacterList.Rendered += _controller.UIPartyOrganization.HandleListInteractable;
            
            _getInPartyNftCharacters = ActionDispatcher.Bind<GetInPartyNftCharactersSucceed>(GetInPartyCharacters);
            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(GetInGameCharacters);
            ActionDispatcher.Dispatch(new GetCharacters());

            _controller.MerchantInputManager.CancelEvent += CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
        }

        protected override void OnExit()
        {
            UITavernItem.Pressed -= _controller.UIPartyOrganization.Transfer;
            UICharacterList.Rendered -= _controller.UIPartyOrganization.HandleListInteractable;
            
            ActionDispatcher.Unbind(_getInPartyNftCharacters);
            ActionDispatcher.Unbind(_getGameDataSucceedEvent);

            _controller.MerchantInputManager.CancelEvent -= CancelPartyOrganization;
            _controller.MerchantInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent -= ResetTransferRequested;
        }

        private void GetInPartyCharacters(GetInPartyNftCharactersSucceed obj)
        {
            _cachedInPartyCharactersData = obj.InPartyCharacters;
            if (obj.InPartyCharacters.Count <= 0) return;
            _controller.UIParty.SetData(obj.InPartyCharacters);
        }

        private void GetInGameCharacters(GetGameNftCharactersSucceed obj)
        {
            _cachedNonPartyCharactersData = obj.InGameCharacters;
            if (obj.InGameCharacters.Count <= 0) return;
            _controller.UINonParty.SetData(obj.InGameCharacters);
        }

        private void CancelPartyOrganization()
        {
            _controller.UIPartyOrganization.gameObject.SetActive(false);
            _controller.UIPartyOrganization.Contents.SetActive(false);
            StateMachine.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UIPartyOrganization.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            StateMachine.Play(ConfirmState);
        }

        private void ResetTransferRequested()
        {
            _controller.UIParty.SetData(_cachedInPartyCharactersData);
            _controller.UINonParty.SetData(_cachedNonPartyCharactersData);
        }
    }
}