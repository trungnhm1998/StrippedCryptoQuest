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
    public class TransferCharactersToGame : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendCharactersGameEvent;

        private void OnEnable()
        {
            _sendCharactersGameEvent = ActionDispatcher.Bind<SendCharactersToGame>(CallAPI);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendCharactersGameEvent);
        }

        private void CallAPI(SendCharactersToGame obj)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "ids", obj.SelectedInWalletCharacters }
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(API.PUT_CHARACTERS_TO_GAME)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed());
        }

        private void OnError(Exception obj)
        {
            Debug.Log("TransferCharactersToGameFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCompleted() { }
    }
}