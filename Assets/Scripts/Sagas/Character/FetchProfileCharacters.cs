using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Character.Hero;
using CryptoQuest.Events.UI.Menu;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CharacterObject = CryptoQuest.Sagas.Objects.Character;

namespace CryptoQuest.Sagas.Character
{
    [Obsolete("Using HeroesInventoryLoader instead.")]
    public class FetchProfileCharacters : SagaBase<FetchProfileCharactersAction>
    {
        [SerializeField] private AssetReferenceT<HeroInventorySO> _heroInventoryAsset;
        [SerializeField] private PartySO _party;
        [SerializeField] private HeroInventoryFilledEvent _inventoryFilled;

        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;
        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private HeroInventorySO _heroInventory;

        protected override void HandleAction(FetchProfileCharactersAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<CharactersResponse>(CharacterAPI.CHARACTERS)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(CharactersResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseCharacters = obj.data.characters;
            if (responseCharacters.Length == 0) return;

            FilterHeroesByStatus(responseCharacters);
        }

        private void FilterHeroesByStatus(CharacterObject[] heroResponseList)
        {
            if (heroResponseList.Length == 0) return;

            var dboxHeroes = heroResponseList.Where(hero => hero.inGameStatus == (int)ECharacterStatus.InBox)
                .ToArray();
            ActionDispatcher.Dispatch(new GetInDboxHeroesSucceeded(dboxHeroes));

            List<CharacterObject> inGameHeroes = new();
            foreach (var hero in heroResponseList)
            {
                if (hero.isHero == 1) continue;
                if (hero.inGameStatus == (int)ECharacterStatus.InGame) inGameHeroes.Add(hero);
            }

            ActionDispatcher.Dispatch(new GetInGameHeroesSucceeded(inGameHeroes.ToArray()));

            OnInventoryFilled(inGameHeroes.ToArray());

            var inPartyHeroes = new List<CharacterObject>();
            foreach (var hero in heroResponseList)
            {
                if (hero.partyId == 0) continue;
                inPartyHeroes.Add(hero);
            }
        }

        private void OnInventoryFilled(CharacterObject[] heroResponseList)
            => StartCoroutine(CoLoadAndUpdateInventory(heroResponseList));

        private IEnumerator CoLoadAndUpdateInventory(CharacterObject[] heroResponseList)
        {
            if (_heroInventory == null)
            {
                var handle = _heroInventoryAsset.LoadAssetAsync();
                yield return new WaitUntil(() => handle.Result != null);
                _heroInventory = handle.Result;
            }

            var converter = ServiceProvider.GetService<IHeroResponseConverter>();
            var nftHeroes = new List<HeroSpec>();
            _heroInventory.OwnedHeroes.Clear();
            foreach (var heroResponse in heroResponseList)
            {
                if (heroResponse.id == -1) continue;
                nftHeroes.Add(converter.Convert(heroResponse));
            }

            _heroInventory.OwnedHeroes = nftHeroes;
            _inventoryFilled.RaiseEvent(_heroInventory.OwnedHeroes);
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::FetchProfileCharacters::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}