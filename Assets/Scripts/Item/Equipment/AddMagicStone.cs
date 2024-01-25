using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Item.MagicStone.Sagas;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    public class AddMagicStone : SagaBase<AddMagicStoneAction>
    {
        [SerializeField] private MagicStoneInventory _inventory;

        private class Body
        {
            [JsonProperty("stoneId")]
            public string Id;

            [JsonProperty("quantity")]
            public int Quantity;
        }

        protected override void HandleAction(AddMagicStoneAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new Body { Id = ctx.Id, Quantity = ctx.Quantity })
                .Post<MagicStonesResponse>(MagicStoneAPI.GET_MAGIC_STONE)
                .Subscribe(OnAddMagicStones, OnError);
        }

        private void OnError(Exception ex)
        {
            Debug.Log($"AddMagicStones::OnError: {ex}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnAddMagicStones(MagicStonesResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new ShowLoading(false));
            var converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var stoneData in response.data.stones)
            {
                _inventory.MagicStones.Add(converter.Convert(stoneData));
            }
        }
    }
}