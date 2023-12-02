using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Actions;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Sagas.MagicStone
{
    public class GetMagicStones : MonoBehaviour
    {
        [SerializeField] private MagicStoneDefinitionDatabase _magicStoneDefinitionDatabase;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
        [SerializeField] private MagicStoneInventorySo _stoneInventory;

        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private MagicStoneResponseConverter _converter;

        private void Awake()
        {
            _converter = new MagicStoneResponseConverter(_magicStoneDefinitionDatabase, _passiveAbilityDatabase);
        }

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileCharactersAction>(HandleAction);
            // _heroInventoryUpdateEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(RefreshHeroInventory);
            // _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
        }

        private void HandleAction(FetchProfileCharactersAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>()
                    { { "source", $"{((int)ECharacterStatus.InGame).ToString()}" } })
                .Get<MagicStonesResponse>(API.Profile.MAGIC_STONE)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(MagicStonesResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;
            var responseStones = obj.data.stones;
            if (responseStones.Length == 0) return;
            OnInventoryFilled(responseStones);
        }

        private void OnInventoryFilled(Objects.MagicStone[] characters)
            => StartCoroutine(CoLoadAndUpdateInventory(characters));

        private IEnumerator CoLoadAndUpdateInventory(Objects.MagicStone[] stonesData)
        {
            // var stones = stonesData.Select(CreateNftCharacter).ToList();
            // _stoneInventory.MagicStones.Clear();
            // _stoneInventory.MagicStones = stones;

            // _inventoryFilled.RaiseEvent(_heroInventory.OwnedHeroes);
            _stoneInventory.MagicStones.Clear();
            foreach (var stoneResponse in stonesData)
            {
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId1);
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId2);
                _stoneInventory.MagicStones.Add(_converter.Convert(stoneResponse));
            }
        }

        private Item.MagicStone.MagicStone CreateNftCharacter(Objects.MagicStone characterResponse)
        {
            var nftCharacter = new Item.MagicStone.MagicStone();
            // FillCharacterData(characterResponse, ref nftCharacter);
            return nftCharacter;
        }

        private void OnError(Exception error)
        {
            Debug.LogError("FetchProfileCharacters::OnError " + error);
        }
    }
}