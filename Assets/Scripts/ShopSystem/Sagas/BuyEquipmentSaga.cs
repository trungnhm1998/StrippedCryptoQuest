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
        [SerializeField] private EquipmentsMinStatsSO _equipmentsMinStatsSO;
        [Serializable]
        struct Body
        {
            [JsonProperty("itemId")]
            public string ItemId;
        }

        protected override void HandleAction(BuyEquipmentAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithoutDispactError()
                .WithBody(new Body
                {
                    ItemId = ctx.Item.Data.ID
                })
                .Post<BuyResponse>(API.BUY)
                .Subscribe(response => BuySuccess(response, ctx.Item), OnError);
        }

        private void BuySuccess(BuyResponse response, IEquipment item)
        {
            var boughtEquipment = response.data.equipments[0];
            var equipment = new Equipment()
            {
                Id = boughtEquipment.id,
                Data = item.Data,
                IsNft = false,
            };
            _equipmentsMinStatsSO.EquipmentsMinStats.TryAdd(equipment.Id, equipment.Stats);
            
            ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
            ActionDispatcher.Dispatch(new FetchProfileAction());
            ActionDispatcher.Dispatch(new TransactionSucceedAction());
        }

        private void OnError(Exception ex)
        {
            ActionDispatcher.Dispatch(new NotEnoughGoldAction());
        }
    }

    [Serializable]
    public class BuyResponse : CommonResponse
    {
        public Data data;

        [Serializable]
        public class Data
        {
            public EquipmentResponse[] equipments;
        }
    }
}