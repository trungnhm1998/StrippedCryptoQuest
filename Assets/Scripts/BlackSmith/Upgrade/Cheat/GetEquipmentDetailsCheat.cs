using System;
using System.Net;
using CommandTerminal;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System.Cheat;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Cheat
{
    public class GetEquipmentDetailsCheat : MonoBehaviour, ICheatInitializer
    {
        private const string GET_EQUIPMENT_DETAILS = "crypto/equipments";
        private IRestClient _restAPINetworkController;

        public void InitCheats()
        {
            Debug.Log("GetBeastDetailsCheat::InitCheats()");
            Terminal.Shell.AddCommand("get.equipments", GetDetailsHandler, 0, 0, "Get Equipment Details");
        }

        private void GetDetailsHandler(CommandArg[] args)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .Get<EquipmentsResponse>(GET_EQUIPMENT_DETAILS)
                .Subscribe(OnHandlerSucceed, OnHandlerFailed);
        }

        private void OnHandlerSucceed(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"Get Equipment Details:: Success");
            GetDetails(response);
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnHandlerFailed(Exception obj)
        {
            Debug.Log($"Get Equipment Details:: Failed : {obj.Message}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void GetDetails(EquipmentsResponse response)
        {
            foreach (var data in response.data.equipments)
            {
                Debug.Log($"Id: {data.id} / Equipment Id: {data.equipmentId} / Level: {data.lv} / Max lv: {data.maxLv}");
            }
        }
    }
}