using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIProfile = CryptoQuest.API.Profile;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileEquipments : SagaBase<FetchProfileEquipmentsAction>
    {
        [SerializeField] private InventoryControllerSO _inventoryController;

        protected override void HandleAction(FetchProfileEquipmentsAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>
                    { { "source", $"{((int)EEquipmentStatus.InGame).ToString()}" } })
                .Get<EquipmentsResponse>(APIProfile.EQUIPMENTS)
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
            _inventoryController.Inventory.NftEquipments.Clear();
            _inventoryController.Inventory.Equipments.Clear();
            foreach (var equipmentResponse in responseEquipments)
            {
                var equipment = converter.Convert(equipmentResponse);
                equipment.AddToInventory(_inventoryController);
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