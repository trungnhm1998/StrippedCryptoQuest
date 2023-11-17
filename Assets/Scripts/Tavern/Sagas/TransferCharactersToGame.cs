using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.System;
using CryptoQuest.Tavern.Objects;
using CryptoQuest.UI.Actions;
using Newtonsoft.Json;
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

            Debug.Log($"SendCharactersToGame::Body={JsonConvert.SerializeObject( body )}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(Profile.PUT_CHARACTERS_TO_GAME)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.characters));
        }

        private void OnError(Exception obj)
        {
            Debug.LogError("TransferCharactersToGameFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}