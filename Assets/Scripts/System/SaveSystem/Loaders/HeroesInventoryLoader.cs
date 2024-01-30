using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Character.Hero;
using CryptoQuest.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class HeroesInventoryLoader : Loader
    {
        [SerializeField] private HeroInventorySO _inventory;

        public override IEnumerator LoadAsync()
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithParam("source", "2") // in game only
                .Get<CharactersResponse>(CharacterAPI.CHARACTERS)
                .ToYieldInstruction();
            yield return op;

            var converter = ServiceProvider.GetService<IHeroResponseConverter>();
            var nftHeroes = new List<HeroSpec>();
            _inventory.OwnedHeroes.Clear();
            var dataCharacters = op.Result.data.characters;
            foreach (var heroResponse in dataCharacters)
            {
                nftHeroes.Add(converter.Convert(heroResponse));
            }

            _inventory.OwnedHeroes = nftHeroes;
        }
    }
}