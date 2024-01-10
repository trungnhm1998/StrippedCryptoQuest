using System;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddGoldRequest : SagaBase<AddGoldAction>
    {
        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("gold")]
            public int Gold;
        }

        protected override void HandleAction(AddGoldAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Gold = ctx.Amount })
                .Post<ProfileResponse>(Reward.ADD_GOLD)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void OnAddSucceed(ProfileResponse response) { }

        private void OnAddFailed(Exception exception) { }
    }
}