using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using System;

namespace CryptoQuest.Inventory.LootAPI
{
    public class ConsumeItemRequest : SagaBase<ItemConsumed>
    {
        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("itemId")]
            public string Id;

            [JsonProperty("itemNum")]
            public int Amount;
        }

        protected override void HandleAction(ItemConsumed ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Id = ctx.Item.ID, Amount = 1 })
                .Post<ItemsResponse>(Consumable.CONSUME)
                .Subscribe(_ => {}, (Exception ex) => OnFail(ex, ctx));
        }

        private void OnFail(Exception _, ItemConsumed ctx)
        {
            ActionDispatcher.Dispatch(new AddConsumableAction(ctx.Item, 1));
        }
    }
}