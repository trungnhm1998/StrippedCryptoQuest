using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Character.Hero;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Events.UI.Menu;
using CryptoQuest.Inventory;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CharacterObject = CryptoQuest.Sagas.Objects.Character;

namespace CryptoQuest.Tavern.Sagas
{
    public class FetchProfileCharacters : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<HeroInventorySO> _heroInventoryAsset;
        private HeroInventorySO _heroInventory;
        [SerializeField] private HeroInventoryFilledEvent _inventoryFilled;

        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;
        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileCharactersAction>(HandleAction);
            _heroInventoryUpdateEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(RefreshHeroInventory);
            _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
            ActionDispatcher.Unbind(_heroInventoryUpdateEvent);
            ActionDispatcher.Unbind(_transferSuccessEvent);
        }

        private void FilterAndRefreshInventory(TransferSucceed ctx)
        {
            var ingameCharacters =
                ctx.ResponseCharacters.Where(character => character.inGameStatus == (int)ECharacterStatus.InGame);
            OnInventoryFilled(ingameCharacters.ToArray());
        }

        private void RefreshHeroInventory(GetGameNftCharactersSucceed ctx)
        {
            OnInventoryFilled(ctx.InGameCharacters.ToArray());
        }

        private void HandleAction(FetchProfileCharactersAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>()
                    { { "source", $"{((int)ECharacterStatus.InGame).ToString()}" } })
                .Get<CharactersResponse>(CharacterAPI.GET_CHARACTERS)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(CharactersResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;
            var responseCharacters = obj.data.characters;
            if (responseCharacters.Length == 0) return;
            OnInventoryFilled(responseCharacters);
        }

        private void OnInventoryFilled(CharacterObject[] heroResponseList)
            => StartCoroutine(CoLoadAndUpdateInventory(heroResponseList));

        private IEnumerator CoLoadAndUpdateInventory(CharacterObject[] heroResponseList)
        {
            if (_heroInventory == null)
            {
                var handle = _heroInventoryAsset.LoadAssetAsync();
                yield return handle;
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
            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }
    }
}