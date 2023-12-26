using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Tavern.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Tavern.Sagas
{
    public class TransferCharacters : SagaBase<TransferCharactersAction>
    {
        protected override void HandleAction(TransferCharactersAction ctx)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "game", ctx.SelectedInDboxCharacters },
                { "wallet", ctx.SelectedInGameCharacters }
            };

            Debug.Log($"SendCharactersToBothSide::Body={JsonConvert.SerializeObject( body )}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(CharacterAPI.TRANSFER)
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed(response.data.characters));
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Tavern::Saga::TransferCharactersToBothSideFailed::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new TransferFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}