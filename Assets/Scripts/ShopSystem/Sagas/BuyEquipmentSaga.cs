using System;
using CryptoQuest.Inventory.Actions;
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
    public class BuyEquipmentAction : ActionBase
    {
        public IEquipment Item { get; }

        public BuyEquipmentAction(IEquipment itemInfo)
        {
            Item = itemInfo;
        }
    }

    public class BuyEquipmentSaga : SagaBase<BuyEquipmentAction>
    {
        [Serializable]
        struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;
        }

        protected override void HandleAction(BuyEquipmentAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new Body
                {
                    ItemId = ctx.Item.Data.ID
                })
                .Post<CommonResponse>(API.BUY)
                .Subscribe(_ => BuySuccess(ctx.Item), (ex) => BuyFailed(ex, ctx.Item));
        }

        private void BuySuccess(IEquipment item)
        {
            ActionDispatcher.Dispatch(new AddEquipmentAction(item));
            ActionDispatcher.Dispatch(new FetchProfileAction());
        }

        private void BuyFailed(Exception exception, IEquipment item) { }
    }
}