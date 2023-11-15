using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharactersInParty : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<HeroInventorySO> _heroInventoryAsset;

        private List<Obj.Character> _charactersInParty = new();
        private List<Obj.Character> _charactersInGameAfterFiltered = new();

        private TinyMessageSubscriptionToken _getInGameNftCharactersSucceededEvent;
        private IPartyController _partyController;
        private HeroInventorySO _heroInventorySO;

        private void OnEnable()
        {
            _getInGameNftCharactersSucceededEvent = ActionDispatcher.Bind<GetGameNftCharactersSucceed>(FilterHeroesInPartyThatExistInListResponse);
            StartCoroutine(CoLoadAssetAsync());
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_getInGameNftCharactersSucceededEvent);
        }

        private IEnumerator CoLoadAssetAsync()
        {
            var handle = _heroInventoryAsset.LoadAssetAsync();
            yield return handle;
            _heroInventorySO = handle.Result;
        }

        private int _count = 0;
        private void FilterHeroesInPartyThatExistInListResponse(GetGameNftCharactersSucceed ctx)
        {
            Debug.Log($"GetNftCharacters::{ctx.InGameCharacters.Count}");

            _charactersInParty.Clear();
            _charactersInGameAfterFiltered.Clear();
            _partyController = ServiceProvider.GetService<IPartyController>();

            foreach (var charInParty in _partyController.Slots)
            {
                if (charInParty.IsValid() == false) _count++;
            }

            _charactersInGameAfterFiltered = ctx.InGameCharacters;

            for (int idx = 0; idx < ctx.InGameCharacters.Count; idx++)
            {
                foreach (var charInParty in _partyController.Slots)
                {
                    Debug.Log($"5 @@@@@@ _charactersInGameAfterFiltered::{ctx.InGameCharacters.Count}");

                    if (charInParty.HeroBehaviour.Spec.Id == 0 && _count >= 3)
                    {
                        Debug.Log($"@@@@@@ 3 _charactersInGameAfterFiltered={_charactersInGameAfterFiltered.Count}");
                        break;
                    }
                    
                    if (_count >= _partyController.Slots.Length)
                    {
                        Debug.Log($"@@@@@@ 2 _charactersInGameAfterFiltered={_charactersInGameAfterFiltered.Count}");
                        break;
                    }
                    
                    if (charInParty.HeroBehaviour.Spec.Id == ctx.InGameCharacters[idx].id)
                    {
                        _charactersInParty.Add(ctx.InGameCharacters[idx]);
                        // _heroInventorySO.OwnedHeroes.RemoveAt(idx); // dont know why it doesnt work
                        _charactersInGameAfterFiltered.RemoveAt(idx);
                        Debug.Log($"@@@@@@ 4 _charactersInParty={_charactersInParty.Count} -- _heroInventorySO.OwnedHeroes.Count={_heroInventorySO.OwnedHeroes.Count}");
                    }

                    if (charInParty.HeroBehaviour.Spec.Id != ctx.InGameCharacters[idx].id)
                    {
                        if (_charactersInGameAfterFiltered.Contains(ctx.InGameCharacters[idx])) continue;
                        _charactersInGameAfterFiltered.Add(ctx.InGameCharacters[idx]);
                        // Debug.Log($"@@@@@@ _charactersInGameAfterFiltered::{_charactersInGameAfterFiltered.Count}");
                    }
                }
            }

            ActionDispatcher.Dispatch(new GetFilteredInGameNftCharactersSucceed(_charactersInGameAfterFiltered));
            ActionDispatcher.Dispatch(new GetInPartyNftCharactersSucceed(_charactersInParty));
        }

    }
}