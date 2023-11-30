using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Character.Sagas
{
    public class FetchInGameNftHeroes : SagaBase<GetInGameHeroes>
    {
        protected override void HandleAction(GetInGameHeroes ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>()
                    { { "source", $"{((int)Obj.ECharacterStatus.InGame).ToString()}" } })
                .Get<Obj.CharactersResponse>(Profile.GET_CHARACTERS)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(Obj.CharactersResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;
            var responseCharacters = obj.data.characters;
            if (responseCharacters.Length == 0) return;
            ActionDispatcher.Dispatch(new FetchInGameHeroesSucceeded(responseCharacters.ToList()));
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.LogError(obj);
            ActionDispatcher.Dispatch(new FetchInGameHeroesFailed());
        }
    }
}