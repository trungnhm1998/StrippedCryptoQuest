using System;
using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Networking;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.Profile
{
    public class RefreshTokenAction : ActionBase { }

    public class RefreshTokenFailed : ActionBase { }

    public class RefreshTokenSaga : SagaBase<RefreshTokenAction>
    {
        [SerializeField] private Credentials _credentials;

        protected override void HandleAction(RefreshTokenAction ctx)
        {
            var restController = ServiceProvider.GetService<IRestClient>();
            restController
                .WithBody(new Dictionary<string, string>()
                {
                    { "refreshToken", _credentials.RefreshToken }
                })
                .WithoutDispactError()
                .Post<AuthResponse>(Accounts.REFRESH_TOKENS)
                .Subscribe(Success, Failed);
        }

        private void Success(AuthResponse ctx)
        {
            var credentialResponse = ctx.data;
            var dataToken = credentialResponse.token;
            _credentials.RefreshToken = dataToken.refresh.token;
            _credentials.Token = dataToken.access.token;
            _credentials.UUID = credentialResponse.user.id;
            _credentials.Wallet = credentialResponse.user.walletAddress;
            _credentials.Save();

            ActionDispatcher.Dispatch(new AuthenticateSucceed());
        }

        private void Failed(Exception _)
        {
            _credentials.RefreshToken = _credentials.Token = _credentials.Wallet = _credentials.UUID = "";
            ActionDispatcher.Dispatch(new RefreshTokenFailed());
        }
    }
}