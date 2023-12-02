using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [SerializeField] private AssetReferenceT<MagicStoneInventorySo> _magicStoneInventoryAsset;

        private MagicStoneInventorySo _stoneInventory;

        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _heroInventoryUpdateEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private IStoneInventoryController _stoneInventoryController;

        private void OnEnable()
        {
            _stoneInventoryController ??= ServiceProvider.GetService<IStoneInventoryController>();
            _fetchEvent = ActionDispatcher.Bind<FetchProfileCharactersAction>(HandleAction);
            // _heroInventoryUpdateEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(RefreshHeroInventory);
            // _transferSuccessEvent = ActionDispatcher.Bind<TransferSucceed>(FilterAndRefreshInventory);
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
            if (_stoneInventory == null)
            {
                var handle = _magicStoneInventoryAsset.LoadAssetAsync();
                yield return handle;
                _stoneInventory = handle.Result;
            }

            // var stones = stonesData.Select(CreateNftCharacter).ToList();
            // _stoneInventory.MagicStones.Clear();
            // _stoneInventory.MagicStones = stones;

            // _inventoryFilled.RaiseEvent(_heroInventory.OwnedHeroes);

            _stoneInventoryController.Add(new Item.MagicStone.MagicStone());
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