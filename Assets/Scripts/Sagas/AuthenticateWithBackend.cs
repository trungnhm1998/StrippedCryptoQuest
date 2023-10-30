using System;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Networking.API;
using CryptoQuest.System;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class AuthenticateWithBackend : SagaBase<AuthenticateWithBackendAction>
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("token")]
            public string Token;
        }

        protected override void HandleAction(AuthenticateWithBackendAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Post<AuthResponse>(Accounts.LOGIN, new Body { Token = ctx.Token })
                .Subscribe(Authenticated, OnError, OnCompleted);
        }

        private void Authenticated(AuthResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK)
            {
                ActionDispatcher.Dispatch(new AuthenticateFailed());
                Debug.Log(JsonConvert.SerializeObject(response));
                return;
            }

            ActionDispatcher.Dispatch(new InternalAuthenticateAction(response.data));
        }

        private void OnError(Exception obj)
        {
            Debug.Log("Auth Failed with error: " + obj.Message);
        }

        private void OnCompleted() { }
    }
}