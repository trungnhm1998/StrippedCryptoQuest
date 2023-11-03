using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.Tavern.Objects;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Tavern.Sagas
{
    public class TransferCharactersToWallet : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendCharactersToWalletEvent;

        private void OnEnable()
        {
            _sendCharactersToWalletEvent = ActionDispatcher.Bind<SendCharactersToWallet>(CallAPI);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendCharactersToWalletEvent);
        }

        private void CallAPI(SendCharactersToWallet obj)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "ids", obj.SelectedInGameCharacters }
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Put<TransferResponse>(API.PUT_CHARACTERS_TO_WALLET, body)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed());
            ActionDispatcher.Dispatch(new GetCharacters { ForceRefresh = true });
        }

        private void OnError(Exception obj) => ActionDispatcher.Dispatch(new ShowLoading(false));

        private void OnCompleted() { }
    }
}