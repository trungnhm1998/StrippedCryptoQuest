using System;
using CryptoQuest.Actions;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
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
        public float diamond;
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
            var token = ctx.ResponseCredentialResponse.token;
            _credentials.Wallet = ctx.ResponseCredentialResponse.user.walletAddress;
            _credentials.UUID = ctx.ResponseCredentialResponse.user.id;
            _credentials.Token = token.access.token;
            _credentials.RefreshToken = token.refresh.token;
            
            if (_credentials.IsLoggedIn() == false)
            {
                ActionDispatcher.Dispatch(new AuthenticateFailed());
                return;
            }

            _credentials.Save();
            ActionDispatcher.Dispatch(new AuthenticateSucceed());
        }
    }
}