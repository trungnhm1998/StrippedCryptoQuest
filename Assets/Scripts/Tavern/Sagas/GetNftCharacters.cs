using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.Tavern.Objects;
using CryptoQuest.UI.Actions;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharacters : SagaBase<NftCharacterAction>
    {
        private readonly List<ICharacterData> _inGameCharacters = new();
        private readonly List<ICharacterData> _walletCharacters = new();

        protected override void HandleAction(NftCharacterAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>() { { "source", $"{((int)ctx.Status).ToString()}" } })
                .Get<CharactersResponse>(API.CHARACTERS)
                .Subscribe(OnGetCharacters, OnError);
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.LogError(obj);
            ActionDispatcher.Dispatch(new GetNftCharactersFailed());
        }

        private void OnGetCharacters(CharactersResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.characters);
            UpdateInboxCache(response.data.characters);

            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void UpdateInGameCache(Objects.Character[] characters)
        {
            if (characters.Length == 0) return;
            _inGameCharacters.Clear();
            foreach (var character in characters)
            {
                if (character.inGameStatus != (int)ETavernStatus.InGame) continue;
                _inGameCharacters.Add(new CharacterData(character.name, character.level, false)
                {
                    Id = character.id
                });
            }

            ActionDispatcher.Dispatch(new GetGameNftCharactersSucceed(_inGameCharacters));
        }

        private void UpdateInboxCache(Objects.Character[] characters)
        {
            if (characters.Length == 0) return;
            _walletCharacters.Clear();
            foreach (var character in characters)
            {
                if (character.inGameStatus != (int)ETavernStatus.InBox) continue;
                _walletCharacters.Add(new CharacterData(character.name, character.level, false)
                {
                    Id = character.id
                });
            }

            ActionDispatcher.Dispatch(new GetWalletNftCharactersSucceed(_walletCharacters));

        }
    }
}