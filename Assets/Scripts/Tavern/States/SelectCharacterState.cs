using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class SelectCharacterState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<IGameCharacterData> _cachedGameData = new List<IGameCharacterData>();
        private List<IWalletCharacterData> _cachedWalletData = new List<IWalletCharacterData>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();
            _controller.UICharacterReplacement.gameObject.SetActive(true);
            _controller.UICharacterReplacement.StateEntered();

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetInGameNftCharactersSucceed>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftCharactersSucceed>(GetWalletCharacters);
            ActionDispatcher.Dispatch(new NftCharacterAction());

            _controller.TavernInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.TavernInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent += SendItemsRequested;
        }

        private void GetInGameCharacters(GetInGameNftCharactersSucceed obj)
        {
            _cachedGameData = obj.InGameCharacters;
            if (obj.InGameCharacters.Count <= 0) return;
            _controller.UICharacterReplacement.CheckEmptyList(_controller.UIGameList, false);
            _controller.UIGameList.SetGameData(obj.InGameCharacters);
        }

        private void GetWalletCharacters(GetWalletNftCharactersSucceed obj)
        {
            _cachedWalletData = obj.WalletCharacters;
            if (obj.WalletCharacters.Count <= 0) return;
            _controller.UIWalletList.SetWalletData(obj.WalletCharacters);
        }

        private void CancelCharacterReplacement()
        {
            _controller.UICharacterReplacement.gameObject.SetActive(false);
            _controller.UICharacterReplacement.StateExited();
            _animator.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            if (_cachedGameData.Count <= 0 || _cachedWalletData.Count <= 0) return;
            _controller.UICharacterReplacement.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            _animator.Play(ConfirmState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            ActionDispatcher.Unbind(_getGameDataSucceedEvent);
            ActionDispatcher.Unbind(_getWalletDataSucceedEvent);

            _controller.TavernInputManager.CancelEvent -= CancelCharacterReplacement;
            _controller.TavernInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent -= SendItemsRequested;
        }
    }
}