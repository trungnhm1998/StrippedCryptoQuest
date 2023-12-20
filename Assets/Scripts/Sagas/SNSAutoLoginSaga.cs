using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.API;
using CryptoQuest.System;
using Newtonsoft.Json;
using System;
using System.Net;
using CryptoQuest.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UniRx;

namespace CryptoQuest.Sagas
{
    public class SNSAutoLoginSaga : SagaBase<SNSAutoLogin>
    {
        public const string SNS_SAVE_KEY = "sns_tokens";
        private const double EXPIRE_DT_IN_SECOND = 3.0;

        [SerializeField] private Credentials _credentials;

        private TinyMessageSubscriptionToken _loginFinishedToken;
        private TinyMessageSubscriptionToken _loginFailedToken;

        [Serializable]
        public struct Body
        {
            public string refreshToken;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _loginFinishedToken = ActionDispatcher.Bind<AuthenticateSucceed>(OnLoginFinished);
            _loginFailedToken = ActionDispatcher.Bind<AuthenticateFailed>(OnLoginFailed);
        }

        protected override void OnDisable()
        {
            ActionDispatcher.Unbind(_loginFinishedToken);
            ActionDispatcher.Unbind(_loginFailedToken);
            base.OnDisable();
        }

        protected override void HandleAction(SNSAutoLogin ctx)
        {
            var refreshTokenStr = string.Empty;

            // Try refresh token from Credential, ignore expires check for debugging purpose
            var refreshToken = _credentials?.Profile?.token?.refresh;
            if (refreshToken != null && !string.IsNullOrEmpty(refreshToken.token))
            {
                refreshTokenStr = refreshToken.token;
            }
            else
            {
                // Try refresh token from PlayerPrefs
                var tokenString = PlayerPrefs.GetString(SNS_SAVE_KEY, string.Empty);
                if (!string.IsNullOrEmpty(tokenString))
                {
                    refreshToken = JsonConvert.DeserializeObject<Access>(tokenString);
                    if (refreshToken != null)
                    {
                        var nowPlusDt = DateTime.Now.AddSeconds(EXPIRE_DT_IN_SECOND);
                        if (DateTime.Parse(refreshToken.expires).CompareTo(nowPlusDt) > 0)
                        {
                            refreshTokenStr = refreshToken.token;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(refreshTokenStr))
            {
                var restClient = ServiceProvider.GetService<IRestClient>();
                restClient
                    .WithBody(new Body { refreshToken = refreshTokenStr })
                    .Post<AuthResponse>(Accounts.REFRESH_TOKENS)
                    .Subscribe(Authenticated, OnError, OnCompleted);
                return;
            }
            ActionDispatcher.Dispatch(new SNSAutoLoginFailed());
        }

        private void OnLoginFinished(AuthenticateSucceed ctx)
        {
            // Save refresh token to PlayerPrefs
            var credential = ServiceProvider.GetService<Credentials>();
            var token = credential?.Profile.token.refresh;
            if (token != null)
            {
                PlayerPrefs.SetString(SNS_SAVE_KEY, JsonConvert.SerializeObject(token));
            }
        }

        private void OnLoginFailed(AuthenticateFailed ctx)
        {
            PlayerPrefs.SetString(SNS_SAVE_KEY, string.Empty);
        }

        private void Authenticated(AuthResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK)
            {
                ActionDispatcher.Dispatch(new SNSAutoLoginFailed());
                Debug.Log(JsonConvert.SerializeObject(response));
                return;
            }
            ActionDispatcher.Dispatch(new InternalAuthenticateAction(response.data));
        }

        private void OnError(Exception obj)
        {
            Debug.LogWarning($"Auto login failed. Message: {obj.Message}");
            ActionDispatcher.Dispatch(new SNSAutoLoginFailed());
        }

        private void OnCompleted() { }
    }
}