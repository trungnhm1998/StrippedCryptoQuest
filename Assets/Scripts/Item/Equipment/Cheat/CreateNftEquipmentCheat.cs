using System;
using System.Net;
using CommandTerminal;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UniRx;
using Newtonsoft.Json;
using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.API;
using IndiGames.Core.Events;

namespace CryptoQuest.Item.Equipment.Cheat
{
    public class CreateNftEquipmentCheat : MonoBehaviour, ICheatInitializer
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("equipmentId")]
            public string EquipmentId;
        }

        private IRestClient _restAPINetworkController;

        private void OnEnable()
        {
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
        }

        public void InitCheats()
    {
            Debug.Log("AddDebugEquipmentCheat::InitCheats()");
            Terminal.Shell.AddCommand("add.nft.equipment", AddEquipment, 1, 1, "Add Nft Equipment Debug");
        }

        private void AddEquipment(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController
                .WithBody(new Body { EquipmentId = args[0].String })
                .WithHeaders(new Dictionary<string, string> { { "DEBUG_KEY", Profile.DEBUG_KEY } })
                .Post<EquipmentsResponse>(Cheats.CREATE_DEBUG_NFT_EQUIPMENT)
                .Subscribe(CreateNftEquipment, OnCreateFailed, OnCreateSucceed);
        }

        private void CreateNftEquipment(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
        }

        private void OnCreateFailed(Exception obj)
        {
            Debug.Log($"Create New Equipment:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCreateSucceed()
        {
            Debug.Log($"Create New Equipment:: Success");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}