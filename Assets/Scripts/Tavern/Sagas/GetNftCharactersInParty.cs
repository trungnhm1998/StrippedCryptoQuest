using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharactersInParty : MonoBehaviour
    {
        private List<Obj.Character> _filteredHeroesInInventory = new();

        private TinyMessageSubscriptionToken _getInGameNftCharactersSucceededEvent;
        private IPartyController _partyController;

        private void OnEnable()
        {
            // _getInGameNftCharactersSucceededEvent =
            //     ActionDispatcher.Bind<FetchInGameHeroesSucceeded>(RemoveHeroesThatExistsInParty);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_getInGameNftCharactersSucceededEvent);
        }

        // private void RemoveHeroesThatExistsInParty(FetchInGameHeroesSucceeded ctx)
        // {
        //     _filteredHeroesInInventory.Clear();
        //     _partyController = ServiceProvider.GetService<IPartyController>();
        //     _filteredHeroesInInventory = new(ctx.InGameHeroes);
        //     for (int idx = _filteredHeroesInInventory.Count - 1; idx >= 0; idx--)
        //     {
        //         var heroInInventory = _filteredHeroesInInventory[idx];
        //         foreach (var partySlot in _partyController.Slots)
        //         {
        //             if (partySlot.IsValid() == false) continue;
        //             var hero = partySlot.HeroBehaviour;
        //             var heroIdInParty = hero.Spec.Id;
        //             var isMain = heroIdInParty == 0;
        //             if (isMain) continue;
        //             if (heroIdInParty == heroInInventory.id)
        //             {
        //                 _filteredHeroesInInventory.RemoveAt(idx);
        //                 break;
        //             }
        //         }
        //     }

            // ActionDispatcher.Dispatch(new GetFilteredInGameNftCharactersSucceed(_filteredHeroesInInventory));
        // }
    }
}