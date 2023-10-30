using System;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    [Serializable]
    public class AuthResponse
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public CredentialResponse data;
    }

    [Serializable]
    public class CredentialResponse
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

    public class InternalAuthenticate : SagaBase<InternalAuthenticateAction>
    {
        [SerializeField] private Credentials _credentials;

        protected override void HandleAction(InternalAuthenticateAction ctx)
        {
            _credentials.Profile = ctx.ResponseCredentialResponse;
            if (_credentials.Profile == null)
            {
                ActionDispatcher.Dispatch(new AuthenticateFailed());
                return;
            }
            ActionDispatcher.Dispatch(new AuthenticateSucceed());
        }
    }
}