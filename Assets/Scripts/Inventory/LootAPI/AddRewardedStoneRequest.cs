using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.MagicStone.Sagas;
using CryptoQuest.Mappings;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddRewardedStoneRequest : SagaBase<AddRewardedMagicStoneAction>
    {
        [SerializeField] private NameMappingDatabase _magicStoneDatabase;

        private class Body
        {
            [JsonProperty("stoneId")]
            public string Id;

            [JsonProperty("quantity")]
            public int Quantity;
        }

        private IRestClient _restClient;

        protected override void HandleAction(AddRewardedMagicStoneAction ctx)
        {
            if (!IsCorrectStoneSetup(ctx)) return;

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new Body { Id = ctx.Id, Quantity = ctx.Quantity })
                .Post<MagicStonesResponse>(MagicStoneAPI.GET_MAGIC_STONE)
                .Subscribe(OnAddMagicStones, OnError);
        }

        private bool IsCorrectStoneSetup(AddRewardedMagicStoneAction ctx)
        {
            if (ctx.Quantity <= 0) return false;
            var stoneMapping = _magicStoneDatabase.NameMappings;
            foreach (var stone in stoneMapping)
            {
                if (stone.Id == ctx.Id) return true;
            }

            return false;
        }

        private void OnError(Exception ex)
        {
            Debug.Log($"AddMagicStones::OnError: {ex}");
        }

        private void OnAddMagicStones(MagicStonesResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            AddStoneToInventory(response.data.stones);
        }

        private void AddStoneToInventory(MagicStone[] stonesResponse)
        {
            var converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var stoneResponse in stonesResponse)
            {
                if (stoneResponse.id == -1) continue;
                var stone = converter.Convert(stoneResponse);
                ActionDispatcher.Dispatch(new AddStoneAction(stone));
            }
        }
    }
}