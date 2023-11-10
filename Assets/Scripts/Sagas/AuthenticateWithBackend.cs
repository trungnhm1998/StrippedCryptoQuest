using System;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Networking;
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
                .WithBody(new Body { Token = ctx.Token })
                .Post<AuthResponse>(Accounts.LOGIN)
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
            ActionDispatcher.Dispatch(new AuthenticateFailed());
        }

        private void OnCompleted() { }
    }
}