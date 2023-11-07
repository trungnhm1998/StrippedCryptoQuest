using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.Tavern.UI;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class CharacterReplacementState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<ICharacterData> _cachedGameData = new List<ICharacterData>();
        private List<ICharacterData> _cachedWalletData = new List<ICharacterData>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Character Replacement");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();
            _controller.UICharacterReplacement.gameObject.SetActive(true);
            _controller.UICharacterReplacement.StateEntered();

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftCharactersSucceed>(GetWalletCharacters);
            ActionDispatcher.Dispatch(new GetCharacters());

            _controller.TavernInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.TavernInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent += SendItemsRequested;
            _controller.TavernInputManager.ResetEvent += ResetTransferRequested;
            _controller.TavernInputManager.InteractEvent += ViewCharacterDetails;
        }

        private void GetInGameCharacters(GetGameNftCharactersSucceed obj)
        {
            _cachedGameData = obj.InGameCharacters;
            if (obj.InGameCharacters.Count <= 0) return;
            _controller.UIGameList.SetData(obj.InGameCharacters);
        }

        private void GetWalletCharacters(GetWalletNftCharactersSucceed obj)
        {
            _cachedWalletData = obj.WalletCharacters;
            if (obj.WalletCharacters.Count <= 0) return;
            _controller.UIWalletList.SetData(obj.WalletCharacters);
        }

        private void CancelCharacterReplacement()
        {
            _controller.UICharacterReplacement.gameObject.SetActive(false);
            _controller.UICharacterReplacement.StateExited();
            _animator.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UICharacterReplacement.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            _animator.Play(ConfirmState);
        }

        private void ResetTransferRequested()
        {
            _controller.UIGameList.SetData(_cachedGameData);
            _controller.UIWalletList.SetData(_cachedWalletData);
        }

        private void ViewCharacterDetails()
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UITavernItem>().InpectDetails();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            ActionDispatcher.Unbind(_getGameDataSucceedEvent);
            ActionDispatcher.Unbind(_getWalletDataSucceedEvent);

            _controller.TavernInputManager.CancelEvent -= CancelCharacterReplacement;
            _controller.TavernInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.TavernInputManager.ResetEvent -= ResetTransferRequested;
            _controller.TavernInputManager.InteractEvent -= ViewCharacterDetails;
        }
    }
}