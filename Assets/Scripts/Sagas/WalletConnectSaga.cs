using CryptoQuest.Networking;
using CryptoQuest.System;
using IndiGames.Web3.Bridge;
using Newtonsoft.Json;
using System;
using CryptoQuest.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class WalletConnectSaga : SagaBase<ConnectWallet>
    {
        private const string WALLET_CONNECT_URL = "crypto/wallet/connect";
        [SerializeField] private Credentials _credentials;

        [Serializable]
        public struct RequestBody
        {
            [JsonProperty("signature")]
            public string Signature;
        }

        [Serializable]
        public class WalletResponse
        {
            [JsonProperty("code")]
            public int Code;

            [JsonProperty("success")]
            public bool Success;

            [JsonProperty("message")]
            public string Message;

            [JsonProperty("time")]
            public long Time;

            [JsonProperty("data")]
            public WalletData Data;
        }

        [Serializable]
        public struct WalletData
        {
            [JsonProperty("address")]
            public string Address;

            [JsonProperty("metad")]
            public string Metad;
        }

        protected void Web3SignedIn(string signature)
        {
            Debug.Log("Web3SignedIn: " + signature);
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithBody(new RequestBody { Signature = signature })
                .Post<WalletResponse>(WALLET_CONNECT_URL)
                .Subscribe(WalletConnected, OnError, OnCompleted);
        }

        protected void Web3SignedError(string error)
        {
            Debug.Log("Web3SignedError: " + error);
            ActionDispatcher.Dispatch(new ConnectWalletCompleted(false));
        }

        protected override void HandleAction(ConnectWallet ctx) {
            Web3.SignIn(gameObject.name, nameof(Web3SignedIn), nameof(Web3SignedError));
        }

        private void WalletConnected(WalletResponse response)
        {
            Debug.Log("WalletConnected: address = " + response.Data.Address + ", metad = " + response.Data.Metad);
            _credentials.Wallet = response.Data.Address;
            ActionDispatcher.Dispatch(new ConnectWalletCompleted(true));
        }

        private void OnError(Exception obj)
        {
            Debug.Log("Auth Failed with error: " + obj.Message);
            ActionDispatcher.Dispatch(new ConnectWalletCompleted(false));
        }

        private void OnCompleted() { }
    }
}