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
    public class TransferToBothGameAndBox : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendCharactersToBothSideEvent;

        private void OnEnable()
        {
            _sendCharactersToBothSideEvent = ActionDispatcher.Bind<SendCharactersToBothSide>(CallAPI);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendCharactersToBothSideEvent);
        }

        private void CallAPI(SendCharactersToBothSide obj)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "game", obj.SelectedInDboxCharacters },
                { "wallet", obj.SelectedInGameCharacters }
            };

            Debug.Log($"SendCharactersToBothSide::Body={JsonConvert.SerializeObject( body )}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(Profile.PUT_CHARACTERS_TO_BOX_AND_GAME)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.characters));
        }

        private void OnError(Exception obj)
        {
            Debug.LogError("TransferCharactersToBothSideFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}