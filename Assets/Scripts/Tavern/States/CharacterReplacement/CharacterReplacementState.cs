using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.Tavern.UI;
using CryptoQuest.Tavern.UI.CharacterReplacement;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class CharacterReplacementState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<ICharacterData> _cachedGameData = new List<ICharacterData>();
        private List<ICharacterData> _cachedWalletData = new List<ICharacterData>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Character Replacement");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UICharacterReplacement.gameObject.SetActive(true);
            _controller.UICharacterReplacement.Contents.SetActive(true);

            UITavernItem.Pressed += _controller.UICharacterReplacement.Transfer;
            UICharacterList.Rendered += _controller.UICharacterReplacement.HandleListInteractable;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftCharactersSucceed>(GetWalletCharacters);

            _controller.TavernInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.TavernInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.TavernInputManager.ExecuteEvent += SendItemsRequested;
            _controller.TavernInputManager.ResetEvent += ResetTransferRequested;
            _controller.TavernInputManager.InteractEvent += ViewCharacterDetails;

            ActionDispatcher.Dispatch(new GetCharacters());
        }

        private void GetInGameCharacters(GetGameNftCharactersSucceed obj)
        {
            _cachedGameData = obj.InGameCharacters;
            _controller.UIGameList.SetData(obj.InGameCharacters);
        }

        private void GetWalletCharacters(GetWalletNftCharactersSucceed obj)
        {
            _cachedWalletData = obj.WalletCharacters;
            _controller.UIWalletList.SetData(obj.WalletCharacters);
        }

        private void CancelCharacterReplacement()
        {
            _controller.UICharacterReplacement.gameObject.SetActive(false);
            _controller.UICharacterReplacement.Contents.SetActive(false);
            StateMachine.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UICharacterReplacement.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            if (_controller.UICharacterReplacement.SelectedGameItemsIds.Count == 0 &&
                _controller.UICharacterReplacement.SelectedWalletItemsIds.Count == 0) return;
            StateMachine.Play(ConfirmState);
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

        protected override void OnExit()
        {
            UITavernItem.Pressed -= _controller.UICharacterReplacement.Transfer;
            UICharacterList.Rendered -= _controller.UICharacterReplacement.HandleListInteractable;

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