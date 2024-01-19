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
using UnityEngine;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class SellEquipmentAction : ActionBase
    {
        private readonly IEquipment _itemInfo;
        private readonly float _itemPrice;

        public SellEquipmentAction(IEquipment itemInfo, float itemPrice)
        {
            _itemInfo = itemInfo;
            _itemPrice = itemPrice;
        }

        public IEquipment ItemInfo => _itemInfo;

        public float ItemPrice => _itemPrice;
    }

    public class SellEquipmentSaga : SagaBase<SellEquipmentAction>
    {
        [Serializable]
        struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;
            [JsonProperty("id")]
            public int Id;
        }

        protected override void HandleAction(SellEquipmentAction ctx)
        {
            var body = new Body()
            {
                Id = ctx.ItemInfo.Id,
                ItemId = ctx.ItemInfo.Data.ID
            };

            var sellingItem = ctx.ItemInfo;

            ActionDispatcher.Dispatch(new RemoveEquipmentAction(sellingItem));
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<CommonResponse>(API.SELL)
                .Subscribe(_ => SellSuccess(sellingItem), (ex) => SellFailed(ex, sellingItem));
        }

        private void SellSuccess(IEquipment sellingItem)
        {
            ActionDispatcher.Dispatch(new FetchProfileAction());
            Debug.Log($"Sell {sellingItem.Id} success");
        }

        private void SellFailed(Exception ex, IEquipment sellingItem)
        {
            ActionDispatcher.Dispatch(new AddEquipmentAction(sellingItem));
        }
    }
}