using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;

namespace CryptoQuest.Inventory.LootAPI
{
    public class AddEquipmentRequest : SagaBase<AddEquipmentRequestAction>
    {
        private IRestClient _restClient;

        protected override void HandleAction(AddEquipmentRequestAction ctx)
        {
            _restClient = ServiceProvider.GetService<IRestClient>();
            _restClient
                .WithBody(new[]
                {
                    ctx.EquipmentId
                })
                .Post<EquipmentsResponse>(EquipmentAPI.EQUIPMENTS)
                .Subscribe(ProcessResponseEquipments, OnError, OnCompleted);
        }

        private void ProcessResponseEquipments(EquipmentsResponse equipmentsResponse)
        {
            if (equipmentsResponse.code != (int)HttpStatusCode.OK) return;
            var responseEquipments = equipmentsResponse.data.equipments;
            if (responseEquipments.Length == 0) return;
            AddEquipmentToInventory(responseEquipments);
        }

        private void AddEquipmentToInventory(EquipmentResponse[] responseEquipments)
        {
            var converter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            foreach (var equipmentResponse in responseEquipments)
            {
                var equipment = converter.Convert(equipmentResponse);
                ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
            }
        }

        private void OnCompleted() { }

        private void OnError(Exception obj) { }
    }
}