using System;
using System.Collections.Generic;
using System.Net;
using CommandTerminal;
using CryptoQuest.API;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.ChangeClass.API.Cheat
{
    public class CreateChangeClassMaterialCheat : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("materialId")]
            public string MaterialId;
            [JsonProperty("materialNum")]
            public string MaterialNum;
        }

        private IRestClient _restAPINetworkController;

        public void InitCheats()
        {
            Debug.Log("AddMaterialToChangeClassCheat::InitCheats()");
            Terminal.Shell.AddCommand("add.material", AddCharacter, 2, 2, "<Material ID> <Quantity>: Add Material To Change Class Character");
        }

        private void AddCharacter(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { MaterialId = args[0].String, MaterialNum = args[1].String })
                .Post<MaterialResponseData>(Cheats.CREATE_CLASS_MATERIAL)
                .Subscribe(CreateMaterial, OnCreateFailed, OnCreateSucceed);
        }

        private void CreateMaterial(MaterialResponseData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
        }

        private void OnCreateFailed(Exception obj)
        {
            Debug.Log($"Create New Materials:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCreateSucceed()
        {
            Debug.Log($"Create New Materials:: Success");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}