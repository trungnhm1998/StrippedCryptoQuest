using System;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Sagas.Party
{
    public class SaveParty : SagaBase<SavePartyAction>
    {
        protected override void HandleAction(SavePartyAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<CharactersResponse>(CharacterAPI.GET_CHARACTERS)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(CharactersResponse obj)
        {
            throw new NotImplementedException();
        }

        private void OnError(Exception obj)
        {
            throw new NotImplementedException();
        }
    }
}