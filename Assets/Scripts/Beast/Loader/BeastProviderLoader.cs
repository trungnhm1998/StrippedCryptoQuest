using System;
using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Beast.Loader
{
    [Serializable]
    public class BeastProviderLoader : System.SaveSystem.Loaders.Loader
    {
        [SerializeField] private BeastProvider _beastProvider;

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

            foreach (var beast in beastsResponse)
            {
                if (beast.IsEquipped) _beastProvider.EquippingBeast = converter.Convert(beast);
            }
        }
    }
}