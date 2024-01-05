using System;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.Party
{
    public class SyncParty : SagaBase<SyncPartyAction>
    {
        protected override void HandleAction(SyncPartyAction ctx)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "ids", ctx.Party }
            };

            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<CharactersResponse>(CharacterAPI.SAVE_PARTY)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(CharactersResponse charactersResponse)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new SavePartyAction(charactersResponse.data.characters));
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::SyncParty::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }
    }
}