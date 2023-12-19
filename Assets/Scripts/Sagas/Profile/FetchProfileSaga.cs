using System.Net;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIProfile = CryptoQuest.API.Profile;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileAction : ActionBase { }

    public class FetchProfileSaga : SagaBase<FetchProfileAction>
    {
        [SerializeField] private Credentials _credentials;
        
        protected override void HandleAction(FetchProfileAction _)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.Get<ProfileResponse>(APIProfile.GET_PROFILE)
                .Subscribe(UpdateProfileInfo);
        }

        private void UpdateProfileInfo(ProfileResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            
            _credentials.Profile.user.walletAddress = response.data.walletAddress;
            
            ActionDispatcher.Dispatch(new SetGoldAction(response.gold));
            ActionDispatcher.Dispatch(new SetDiamondAction(response.diamond));
            ActionDispatcher.Dispatch(new SetSoulAction(response.soul));
        }
    }
}