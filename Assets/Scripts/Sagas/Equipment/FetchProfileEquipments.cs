using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class FetchProfileEquipments : SagaBase<FetchProfileEquipmentsAction>
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;

        protected override void HandleAction(FetchProfileEquipmentsAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>
                    { { "source", $"{((int)EEquipmentStatus.InGame).ToString()}" } })
                .Get<EquipmentsResponse>(EquipmentAPI.EQUIPMENTS)
                .Subscribe(ProcessResponseEquipments, OnError, OnCompleted);
        }

        private void ProcessResponseEquipments(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            var responseEquipments = response.data.equipments;
            if (responseEquipments.Length == 0) return;
            OnInventoryFilled(responseEquipments);
        }

        private void OnInventoryFilled(EquipmentResponse[] responseEquipments)
        {
            var converter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            _equipmentInventory.Equipments.Clear();
            foreach (var equipmentResponse in responseEquipments)
            {
                var equipment = converter.Convert(equipmentResponse);
                _equipmentInventory.Equipments.Add(equipment);
            }
            ActionDispatcher.Dispatch(new InventoryFilled());
        }

        private void OnError(Exception error)
        {
            Debug.Log("FetchProfileEquipments::OnError " + error);
        }

        private void OnCompleted() => ActionDispatcher.Dispatch(new ShowLoading(false));
    }
}