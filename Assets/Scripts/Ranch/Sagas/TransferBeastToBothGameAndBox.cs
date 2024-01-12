using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Ranch.Object;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    public class TransferBeastToBothGameAndBox : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendCharactersToBothSideEvent;

        private void OnEnable()
        {
            _sendCharactersToBothSideEvent = ActionDispatcher.Bind<SendBeastsToBothSide>(CallAPI);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendCharactersToBothSideEvent);
        }

        private void CallAPI(SendBeastsToBothSide obj)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "game", obj.SelectedInDimensionBoxBeasts.Select(item => item.Id).ToArray() },
                { "wallet", obj.SelectedInGameBeasts.Select(item => item.Id).ToArray() }
            };

            Debug.Log($"SendCharactersToBothSide::Body={JsonConvert.SerializeObject(body)}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(BeastAPI.TRANSFER)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.beasts));
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