﻿using CryptoQuest.Core;
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
    public class AuthenticateUsingWallet : AuthenticationSagaBase<LoginUsingWallet>
    {
        private const string WALLET_CONNECT_URL = "crypto/wallet/connect";

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

            [JsonProperty("gold")]
            public int Gold;

            [JsonProperty("diamond")]
            public int Diamond;

            [JsonProperty("soul")]
            public int Soul;

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
        }

        // TODO: implement
        protected override void HandleAuthenticate(LoginUsingWallet ctx) {
            Web3.SignIn(gameObject.name, nameof(Web3SignedIn), nameof(Web3SignedError));
        }

        private void WalletConnected(WalletResponse response)
        {
            Debug.Log("WalletConnected: address = " + response.Data.Address + ", metad = " + response.Data.Metad);
            ActionDispatcher.Dispatch(new ShowWalletButton(false));
        }

        private void OnError(Exception obj)
        {
            Debug.Log("Auth Failed with error: " + obj.Message);
            ActionDispatcher.Dispatch(new AuthenticateFailed());
        }

        private void OnCompleted() {
            ActionDispatcher.Dispatch(new AuthenticateSucceed());
        }
    }
}