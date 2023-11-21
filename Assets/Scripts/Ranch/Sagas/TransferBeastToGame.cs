using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Ranch.Object;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using Newtonsoft.Json;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    public class TransferBeastToGame : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendBeastsToGameMessenger;

        private void OnEnable()
        {
            _sendBeastsToGameMessenger = ActionDispatcher.Bind<SendBeastsToGame>(CallAPI);
        }

        private void CallAPI(SendBeastsToGame obj)
        {
            Dictionary<string, int[]> body = new Dictionary<string, int[]>()
            {
                { "ids", obj.SelectedInWalletBeasts }
            };

            Debug.Log($"SendBeastToGame::Body={JsonConvert.SerializeObject(body)}");

            IRestClient restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(Profile.PUT_BEASTS_TO_GAME)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.beasts));
        }

        private void OnError(Exception obj)
        {
            Debug.LogError("TransferBeastToGameFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}