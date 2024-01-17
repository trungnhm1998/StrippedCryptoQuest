using System;
using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class MagicStoneInventoryLoader : Loader
    {
        [SerializeField] private MagicStoneInventory _inventory;

        public override IEnumerator LoadAsync()
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithParam("source", "2") // in game only
                .Get<MagicStonesResponse>(MagicStoneAPI.GET_MAGIC_STONE)
                .ToYieldInstruction();
            yield return op;

            var stonesResponse = op.Result.data.stones;
            _inventory.MagicStones.Clear();
            var converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var stoneResponse in stonesResponse)
            {
                _inventory.MagicStones.Add(converter.Convert(stoneResponse));
            }
        }
    }
}