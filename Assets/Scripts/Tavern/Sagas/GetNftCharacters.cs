﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UniRx;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharacters : SagaBase<GetCharacters>
    {
        private readonly List<Obj.Character> _inGameCharacters = new();
        private readonly List<Obj.Character> _walletCharacters = new();

        private List<PartySlotSpec> _partySlotSpecs;

        protected override void HandleAction(GetCharacters ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<Obj.CharactersResponse>(Profile.GET_CHARACTERS)
                .Subscribe(OnGetCharacters, OnError);
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.LogError(obj);
            ActionDispatcher.Dispatch(new GetNftCharactersFailed());
        }

        private void OnGetCharacters(Obj.CharactersResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"GetNftCharacters::response={JsonConvert.SerializeObject(response.data.characters)}");
            UpdateInGameCache(response.data.characters);
            UpdateInboxCache(response.data.characters);

            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void UpdateInGameCache(Obj.Character[] characters)
        {
            if (characters.Length == 0) return;
            _inGameCharacters.Clear();
            foreach (var character in characters)
            {
                if (character.inGameStatus != (int)Obj.ECharacterStatus.InGame) continue;
                _inGameCharacters.Add(character);
            }

            ActionDispatcher.Dispatch(new GetGameNftCharactersSucceed(_inGameCharacters));
            Debug.Log($"GetNftCharacters::{_inGameCharacters.Count}");
        }

        private void UpdateInboxCache(Obj.Character[] characters)
        {
            if (characters.Length == 0) return;
            _walletCharacters.Clear();
            foreach (var character in characters)
            {
                if (character.inGameStatus != (int)Obj.ECharacterStatus.InBox) continue;
                _walletCharacters.Add(character);
            }

            ActionDispatcher.Dispatch(new GetWalletNftCharactersSucceed(_walletCharacters));

        }
    }
}