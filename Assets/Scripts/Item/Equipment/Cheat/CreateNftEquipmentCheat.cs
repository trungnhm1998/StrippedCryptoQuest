using System;
using System.Collections.Generic;
using System.Net;
using CommandTerminal;
using CryptoQuest.API;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Item.Equipment.Cheat
{
    public class CreateNftEquipmentCheat : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;

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
                .Subscribe(OnCreateSucceed, OnCreateFailed);
        }

        private void OnCreateSucceed(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            var converter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            foreach (var equipmentData in response.data.equipments)
            {
                _equipmentInventory.Equipments.Add(converter.Convert(equipmentData));
            }
            
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnCreateFailed(Exception obj)
        {
            Debug.Log($"Create New Equipment:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}