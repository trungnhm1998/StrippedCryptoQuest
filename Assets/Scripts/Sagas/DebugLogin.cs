using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Networking.API;
using CryptoQuest.System;
using Newtonsoft.Json;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class DebugLogin : SagaBase<DebugLoginAction>
    {
        [Serializable]
        private struct DebugBody
        {
            [JsonProperty("token")]
            public string Token;
        }

        [SerializeField] private string _debugToken =
            "c1CRi-qi8jfOJHJ5rjH2tO9xjSA_UUORQ1eRBt59BY8.sc6AO3PQnOrQV0hG4SoQ6mTeU8r1n4-WKuCuzrpnmw1";

        [SerializeField] private string _debugKey = "GQwuFb5HYRrbodgHmlyeJPXYDfRUpxkOZrFlWarb";

        private TinyMessageSubscriptionToken _loginAction;

        protected override void HandleAction(DebugLoginAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Post<AuthResponse>(Account.DEBUG_LOGIN, new DebugBody { Token = _debugToken, },
                    new Dictionary<string, string> { { "DEBUG_KEY", _debugKey }, })
                .Subscribe(SaveCredentials, DispatchLoginFailed, DispatchLoginFinished);
        }

        private void SaveCredentials(AuthResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new InternalAuthenticateAction(response.data));
        }

        private void DispatchLoginFailed(Exception obj)
        {
            ActionDispatcher.Dispatch(new LoginFailedAction());
        }

        private void DispatchLoginFinished()
        {
            ActionDispatcher.Dispatch(new LoginFinishedAction());
        }
    }
}