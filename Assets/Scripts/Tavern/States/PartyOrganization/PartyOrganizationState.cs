using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class PartyOrganizationState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getInPartyNftCharacters;
        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;

        private List<ICharacterData> _cachedInPartyCharactersData = new List<ICharacterData>();
        private List<ICharacterData> _cachedNonPartyCharactersData = new List<ICharacterData>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Party Organization");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();
            _controller.UIPartyOrganization.gameObject.SetActive(true);
            _controller.UIPartyOrganization.StateEntered();

            _getInPartyNftCharacters = ActionDispatcher.Bind<GetInPartyNftCharactersSucceed>(GetInPartyCharacters);
            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(GetInGameCharacters);
            ActionDispatcher.Dispatch(new GetCharacters());

            _controller.TavernInputManager.CancelEvent += CancelPartyOrganization;
            _controller.TavernInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent += SendItemsRequested;
            _controller.TavernInputManager.ResetEvent += ResetTransferRequested;
        }

        private void GetInPartyCharacters(GetInPartyNftCharactersSucceed obj)
        {
            _cachedInPartyCharactersData = obj.InPartyCharacters;
            if (obj.InPartyCharacters.Count <= 0) return;
            _controller.UIPartyOrganization.CheckEmptyList(_controller.UIParty, false);
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
            _controller.UIPartyOrganization.StateExited();
            _animator.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UIPartyOrganization.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            _animator.Play(ConfirmState);
        }

        private void ResetTransferRequested()
        {
            _controller.UIParty.SetData(_cachedInPartyCharactersData);
            _controller.UINonParty.SetData(_cachedNonPartyCharactersData);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            ActionDispatcher.Unbind(_getInPartyNftCharacters);
            ActionDispatcher.Unbind(_getGameDataSucceedEvent);

            _controller.TavernInputManager.CancelEvent -= CancelPartyOrganization;
            _controller.TavernInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.TavernInputManager.ResetEvent -= ResetTransferRequested;
        }
    }
}