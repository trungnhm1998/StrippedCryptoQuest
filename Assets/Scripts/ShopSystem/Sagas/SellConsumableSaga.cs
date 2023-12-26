using System;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class SellConsumableAction : ActionBase
    {
        public ConsumableSO Item { get; }
        public int Quantity { get; }

        public SellConsumableAction(ConsumableSO item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }

    public class SellConsumableSaga : SagaBase<SellConsumableAction>
    {
        [Serializable]
        struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;
            [JsonProperty("itemNum")]
            public int SellQuantity;
        }
        /// <summary>
        /// Add gold and remove item from inventory
        /// </summary>
        /// <param name="ctx"></param>
        protected override void HandleAction(SellConsumableAction ctx)
        {
            Body body = new()
            {
                ItemId = ctx.Item.ID,
                SellQuantity = ctx.Quantity
            };

            ActionDispatcher.Dispatch(new RemoveConsumableAction(ctx.Item, ctx.Quantity));
            
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<CommonResponse>(API.Sell)
                .Subscribe(_ => SellSuccess(ctx), (ex) => SellFailed(ex, ctx));
        }

        private void SellSuccess(SellConsumableAction ctx)
        {
            ActionDispatcher.Dispatch(new FetchProfileAction());
        }
        
        private void SellFailed(Exception exception, SellConsumableAction ctx)
        {
            // TODO: add item back to inventory
        }
    }
}