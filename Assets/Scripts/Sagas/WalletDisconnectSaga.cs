using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Web3.Bridge;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class WalletDisconnectSaga : SagaBase<DisconnectWallet>
    {
        private const string WALLET_DISCONNECT_URL = "crypto/wallet/disconnect";

        [Serializable]
        public class WalletDisconnectResponse
        {
            [JsonProperty("code")]
            public int Code;

            [JsonProperty("success")]
            public bool Success;

            [JsonProperty("message")]
            public string Message;
        }

        protected override void HandleAction(DisconnectWallet ctx) {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Post<WalletDisconnectResponse>(WALLET_DISCONNECT_URL)
                .Subscribe(WalletDisconnected, OnError, OnCompleted);
        }

        private void WalletDisconnected(WalletDisconnectResponse response)
        {
            ActionDispatcher.Dispatch(new DisconnectWalletWalletCompleted(true));
        }

        private void OnError(Exception obj)
        {
            Debug.LogException(obj);
            ActionDispatcher.Dispatch(new DisconnectWalletWalletCompleted(false));
        }

        private void OnCompleted() { }
    }
}