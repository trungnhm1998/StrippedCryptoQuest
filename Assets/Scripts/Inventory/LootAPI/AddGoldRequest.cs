using System;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddGoldToServerAction : ActionBase
    {
        public int Amount { get; }

        public AddGoldToServerAction(int amount)
        {
            Amount = amount;
        }
    }

    public class AddGoldRequest : SagaBase<AddGoldToServerAction>
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _gold;

        private IRestClient _restClient;

        private struct Body
        {
            [JsonProperty("gold")] public int Gold;
        }

        protected override void HandleAction(AddGoldToServerAction ctx)
        {
            var currentGoldAmount = _wallet[_gold].Amount;
            var goldToUpdate = currentGoldAmount + ctx.Amount;
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Gold = (int)goldToUpdate })
                .Post<ProfileResponse>(Reward.COMMON_DATA)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void OnAddSucceed(ProfileResponse response)
        {
            ActionDispatcher.Dispatch(new SetGoldAction(response.gold));
        }

        private void OnAddFailed(Exception exception)
        {
            Debug.LogWarning($"Add currency fail. Log:\n{exception.Message}");
        }
    }
}