using System;
using System.Collections.Generic;
using System.Net;
using CommandTerminal;
using CryptoQuest.API;
using CryptoQuest.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.ChangeClass.API.Cheat
{
    public class CreateCharacterCheat : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private HeroInventorySO _inventory;

        [Serializable]
        public struct Body
        {
            [JsonProperty("unitId")]
            public string UnitId;
        }

        private IRestClient _restAPINetworkController;
        private CharactersResponse _response;

        public void InitCheats()
        {
            Debug.Log("AddDebugCharacterCheat::InitCheats()");
            Terminal.Shell.AddCommand("add.nft.character", AddCharacter, 1, 1, "Add Nft Character Debug");
        }

        private void AddCharacter(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { UnitId = args[0].String })
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Post<CharactersResponse>(Cheats.CREATE_DEBUG_CHARACTER)
                .Subscribe(CreateDebugCharacter, OnCreateFailed, OnCreateSucceed);
        }

        private void CreateDebugCharacter(CharactersResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            _response = response;
        }

        private void OnCreateFailed(Exception obj)
        {
            Debug.Log($"Create New Character:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCreateSucceed()
        {
            Debug.Log($"Create New Character:: Success");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ConvertCharacterResponse(_response.data.characters);
        }

        private void ConvertCharacterResponse(Sagas.Objects.Character[] response)
        {
            var responseConverter = ServiceProvider.GetService<IHeroResponseConverter>();
            foreach (var hero in response)
            {
                var heroSpec = responseConverter.Convert(hero);
                _inventory.Add(heroSpec);
            }
        }
    }
}