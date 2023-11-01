﻿using CryptoQuest.Core;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class CharacterReplacementPresenter : MonoBehaviour
    {
        [SerializeField] private TavernInputManager _inputManager;
        [SerializeField] private TavernDialogsManager _dialogsManager;

        [SerializeField] private UICharacterReplacement _uiCharacterReplacement;
        [SerializeField] private UICharacterListGame _uiCharacterListGame;
        [SerializeField] private UICharacterListWallet _uiCharacterListWallet;

        [Header("Dialog Messages")]
        [SerializeField] private LocalizedString _confirmMessage;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private void OnEnable()
        {
            _inputManager.ExecuteEvent += SendItemsRequested;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetInGameNftCharactersSucceed>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftCharactersSucceed>(GetWalletCharacters);
            ActionDispatcher.Dispatch(new NftCharacterAction());
        }

        private void GetInGameCharacters(GetInGameNftCharactersSucceed obj)
        {
            _uiCharacterListGame.SetGameData(obj.InGameCharacters, true);
        }

        private void GetWalletCharacters(GetWalletNftCharactersSucceed obj)
        {
            _uiCharacterListWallet.SetWalletData(obj.WalletCharacters, true);
        }

        private void OnDisable()
        {
            _inputManager.ExecuteEvent -= SendItemsRequested;
            ActionDispatcher.Unbind(_getGameDataSucceedEvent);
            ActionDispatcher.Unbind(_getWalletDataSucceedEvent);
        }

        private void SendItemsRequested()
        {
            _dialogsManager.ChoiceDialog
                .SetMessage(_confirmMessage)
                .Show();
        }
    }
}