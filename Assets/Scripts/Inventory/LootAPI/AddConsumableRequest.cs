using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddConsumableToServerAction : ConsumableQuantityChangedAction
    {
        public AddConsumableToServerAction(ConsumableSO item, int quantity = 1) : base(item, quantity) { }
    }

    public class AddConsumableRequest : SagaBase<AddConsumableToServerAction>
    {
        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("itemId")]
            public string Id;

            [JsonProperty("itemNum")]
            public int Amount;
        }

        protected override void HandleAction(AddConsumableToServerAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Id = ctx.Item.ID, Amount = ctx.Quantity })
                .Post<ItemsResponse>(Consumable.ITEMS)
                .Subscribe(_ => OnSuccess(ctx));
        }

        private void OnSuccess(AddConsumableToServerAction ctx)
        {
            ActionDispatcher.Dispatch(new AddConsumableAction(ctx.Item, ctx.Quantity));
        }
    }
}