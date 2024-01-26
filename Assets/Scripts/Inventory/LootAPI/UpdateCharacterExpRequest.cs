using System;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.Inventory.LootAPI
{
    public class UpdateCharacterExpRequest : SagaBase<UpdateCharacterExpAction>
    {
        private IRestClient _restClient;

        public struct UpdatePartyEXPBody
        {
            public UpdateEXPBody[] Characters;
        }

        public struct UpdateEXPBody
        {
            [JsonProperty("id")] public int Id;

            [JsonProperty("exp")] public float Exp;
        }

        protected override void HandleAction(UpdateCharacterExpAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(ctx.UpdateEXPRequests)
                .Put<CharactersResponse>(Accounts.CHARACTERS)
                .Subscribe();
        }
    }
}