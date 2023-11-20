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
    public class TransferBeastToDimensionalBox : MonoBehaviour
    {
        private TinyMessageSubscriptionToken _sendBeastsToWalletEvent;

        private void OnEnable()
        {
            _sendBeastsToWalletEvent = ActionDispatcher.Bind<SendBeastsToWallet>(CallAPI);
        }


        private void OnDisable()
        {
            ActionDispatcher.Unbind(_sendBeastsToWalletEvent);
        }

        private void CallAPI(SendBeastsToWallet response)
        {
            Dictionary<string, int[]> body = new Dictionary<string, int[]>()
            {
                { "ids", response.SelectedInGameBeasts }
            };

            Debug.Log($"SendBeastToDBox::Body={JsonConvert.SerializeObject(body)}");

            IRestClient restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(Profile.PUT_BEASTS_TO_DIMENSIONAL_BOX)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.beasts));
        }

        private void OnError(Exception obj)
        {
            Debug.LogError("TransferBeastToDBoxFailed::" + obj);
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}