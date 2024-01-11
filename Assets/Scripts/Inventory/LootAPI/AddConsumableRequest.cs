using System;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddConsumableRequest : SagaBase<AddConsumableAction>
    {
        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("itemId")]
            public string Id;

            [JsonProperty("itemNum")]
            public int Amount;
        }

        protected override void HandleAction(AddConsumableAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Id = ctx.Item.ID, Amount = ctx.Quantity })
                .Post<ItemsResponse>(Consumable.ITEMS)
                .Subscribe();
        }
    }
}