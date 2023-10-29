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
    [Serializable]
    public class DebugLoginResponse
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public UserProfile user;
        public Token token;
    }

    [Serializable]
    public class UserProfile
    {
        public string id;
        public string name;
        public string socialUserName;
        public string email;
        public string avatar_image_url;
        public string walletAddress;
    }

    [Serializable]
    public class Token
    {
        public Access access;
        public Access refresh;
    }

    [Serializable]
    public class Access
    {
        public string token;
        public string expires;
    }

    public class DebugLogin : MonoBehaviour
    {
        [Serializable]
        private struct DebugBody
        {
            [JsonProperty("token")]
            public string Token;
        }

        [SerializeField] private Credentials _credentials;

        [SerializeField] private string _debugToken =
            "c1CRi-qi8jfOJHJ5rjH2tO9xjSA_UUORQ1eRBt59BY8.sc6AO3PQnOrQV0hG4SoQ6mTeU8r1n4-WKuCuzrpnmw1";

        [SerializeField] private string _debugKey = "GQwuFb5HYRrbodgHmlyeJPXYDfRUpxkOZrFlWarb";

        private TinyMessageSubscriptionToken _loginAction;

        private void OnEnable() => _loginAction = ActionDispatcher.Bind<DebugLoginAction>(Login);

        private void OnDisable() => ActionDispatcher.Unbind(_loginAction);

        private void Login(DebugLoginAction _)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Post<DebugLoginResponse>(Account.DEBUG_LOGIN, new DebugBody { Token = _debugToken, },
                    new Dictionary<string, string> { { "DEBUG_KEY", _debugKey }, })
                .Subscribe(SaveCredentials, DispatchLoginFailed, DispatchLoginFinished);
        }

        private void SaveCredentials(DebugLoginResponse response)
        {
            Debug.Log(response.code);
            if (response.code != (int)HttpStatusCode.OK) return;
            _credentials.Save(response.data);
            ActionDispatcher.Dispatch(new AuthenticateSucceed());
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