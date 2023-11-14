using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Tavern.UI;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class CharacterReplacementState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<Obj.Character> _cachedGameData = new List<Obj.Character>();
        private List<Obj.Character> _cachedWalletData = new List<Obj.Character>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Character Replacement");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UICharacterReplacement.gameObject.SetActive(true);
            _controller.UICharacterReplacement.Contents.SetActive(true);
            _controller.UICharacterReplacement.SelectedGameItemsIds.Clear();
            _controller.UICharacterReplacement.SelectedWalletItemsIds.Clear();

            UITavernItem.Pressed += _controller.UICharacterReplacement.Transfer;
            UICharacterList.Rendered += _controller.UICharacterReplacement.HandleListInteractable;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftCharactersSucceed>(GetWalletCharacters);

            _controller.MerchantInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent += ViewCharacterDetails;

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

            _controller.MerchantInputManager.CancelEvent -= CancelCharacterReplacement;
            _controller.MerchantInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent -= ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent -= ViewCharacterDetails;
        }
    }
}