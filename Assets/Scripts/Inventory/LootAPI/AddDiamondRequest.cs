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
    public class AddDiamondRequest : SagaBase<AddDiamonds>
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _diamond;
        private IRestClient _restClient;

        private class Body
        {
            [JsonProperty("diamond")]
            public long Diamond;
        }

        protected override void HandleAction(AddDiamonds ctx)
        {
            var currentDiamondAmount = _wallet[_diamond].Amount;
            var diamondToUpdate = currentDiamondAmount + ctx.Amount;
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new Body { Diamond = (int)diamondToUpdate })
                .Post<ProfileResponse>(Reward.COMMON_DATA)
                .Subscribe(OnAddSucceed, OnAddFailed);
        }

        private void OnAddSucceed(ProfileResponse response)
        {
            ActionDispatcher.Dispatch(new SetDiamondAction(response.diamond));
        }

        private void OnAddFailed(Exception exception)
        {
            Debug.LogWarning($"Add currency fail. Log:\n{exception.Message}");
        }
    }
}