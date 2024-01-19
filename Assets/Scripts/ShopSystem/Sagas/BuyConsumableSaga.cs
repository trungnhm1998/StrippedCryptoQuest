using System;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class BuyConsumableAction : ActionBase
    {
        public ConsumableInfo Item { get; }

        public BuyConsumableAction(ConsumableInfo itemInfo)
        {
            Item = itemInfo;
        }
    }

    public class BuyConsumableSaga : SagaBase<BuyConsumableAction>
    {
        [Serializable]
        struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;

            [JsonProperty("itemNum")]
            public int Quantity;
        }

        protected override void HandleAction(BuyConsumableAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithoutDispactError()
                .WithBody(new Body
                {
                    ItemId = ctx.Item.Data.ID,
                    Quantity = ctx.Item.Quantity
                })
                .Post<CommonResponse>(API.BUY)
                .Subscribe(_ => BuySuccess(ctx.Item), OnError);
        }

        private void BuySuccess(ConsumableInfo item)
        {
            ActionDispatcher.Dispatch(new AddConsumableAction(item.Data, item.Quantity));
            ActionDispatcher.Dispatch(new FetchProfileAction());
            ActionDispatcher.Dispatch(new TransactionSucceedAction());
        }

        private void OnError(Exception exception)
        {
            ActionDispatcher.Dispatch(new MaximumQuantityExceedAction());
        }
    }
}