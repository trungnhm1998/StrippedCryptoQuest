using System;
using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Beast.Loader
{
    [Serializable]
    public class BeastInventoryLoader : System.SaveSystem.Loaders.Loader
    {
        [SerializeField] private BeastInventorySO _inventory;

        public override IEnumerator LoadAsync()
        {
            var restClient = ServiceProvider.GetService<IRestClient>();

            var op = restClient
                .WithParam("source", "2")
                .Get<BeastsResponse>(BeastAPI.GET_BEASTS)
                .ToYieldInstruction();

            yield return op;

            var beastsResponse = op.Result.data.beasts;

            var converter = ServiceProvider.GetService<IBeastResponseConverter>();

            _inventory.OwnedBeasts.Clear();
            foreach (var beast in beastsResponse)
            {
                _inventory.OwnedBeasts.Add(converter.Convert(beast));
            }
        }
    }
}