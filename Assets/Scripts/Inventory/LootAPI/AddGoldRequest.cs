using System;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

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

        private void OnAddSucceed(ProfileResponse response)
        {
            ActionDispatcher.Dispatch(new SetGoldAction(response.gold));
        }

        private void OnAddFailed(Exception exception)
        {
            Debug.LogWarning($"Add currency fail. Log:\n{exception.Message}");
        }
    }
}