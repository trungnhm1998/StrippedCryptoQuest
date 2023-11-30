using System;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIProfile = CryptoQuest.API.Profile;

namespace CryptoQuest.Sagas.Profile
{
    public class GetOnlineProfileWallet : SagaBase<GetProfileSucceed>
    {
        protected override void HandleAction(GetProfileSucceed ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Get<ProfileResponse>(APIProfile.GET_PROFILE)
                .Subscribe(UpdateProfileInfo, OnError);
        }

        private void UpdateProfileInfo(ProfileResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new UpdateWallet(response.gold, response.soul, response.diamond, response.data.walletAddress));
        }

        private void OnError(Exception response)
        {
            Debug.LogError($"UpdateWalletOnGetProfile::OnError [{response}]");
        }
    }
}