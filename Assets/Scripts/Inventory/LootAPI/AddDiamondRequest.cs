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
    public class AddDiamondRequest : SagaBase<AddDiamonds>
    {
        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("diamond")]
            public int Diamond;
        }

        protected override void HandleAction(AddDiamonds ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Diamond = ctx.Amount })
                .Post<ProfileResponse>(Reward.ADD_DIAMOND)
                .Subscribe();
        }
    }
}