using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UnityEngine;
using System;
using UniRx;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class UpdateGoldAction : ActionBase 
    {
        public int Gold { get; private set; }
        public UpdateGoldAction(int gold) => Gold = gold;
    } 

    public class UpdateGoldSaga : SagaBase<UpdateGoldAction>
    {
        public static readonly string UpdateGoldApi = "crypto/user/common-data";
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;

        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("gold")]
            public int Gold;
        }

        protected override void HandleAction(UpdateGoldAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Gold = ctx.Gold })
                .Post<ProfileResponse>(UpdateGoldApi)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void OnAddSucceed(ProfileResponse response)
        {
            ActionDispatcher.Dispatch(new SetGoldAction(response.gold));
        }

        private void OnAddFailed(Exception exception)
        {
            Debug.LogWarning($"Update currency fail. Log:\n{exception.Message}");
        }
    }
}