using System;
using System.Collections.Generic;
using System.Net;
using CommandTerminal;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Beast.Cheat
{
    public class CreateDebugBeastCheat : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("beastId")]
            public string BeastId;
        }

        private IRestClient _restAPINetworkController;

        public void InitCheats()
        {
            Debug.Log("AddDebugBeastCheat::InitCheats()");
            Terminal.Shell.AddCommand("add.nft.beast", AddCharacter, 1, 1, "Add Nft Beast Debug");
        }

        private void AddCharacter(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { BeastId = args[0].String })
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Post<BeastsResponse>(Cheats.CREATE_DEBUG_BEAST)
                .Subscribe(CreateDebugCharacter, OnCreateFailed, OnCreateSucceed);
        }

        private void CreateDebugCharacter(BeastsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
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
        }
    }
}