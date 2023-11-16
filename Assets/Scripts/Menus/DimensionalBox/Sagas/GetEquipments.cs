using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class GetEquipments : SagaBase<GetNftEquipments>
    {
        [SerializeField] private GetEquipmentsEvent _inGameEvent;
        [SerializeField] private GetEquipmentsEvent _inBoxEvent;

        // TODO: Not good practice to save data in saga
        private readonly List<EquipmentResponse> _inGameEquipmentsCache = new();
        private readonly List<EquipmentResponse> _inBoxEquipmentsCache = new();

        protected override void HandleAction(GetNftEquipments ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>() { { "source", $"{((int)ctx.Status).ToString()}" } })
                .Get<EquipmentsResponse>(Profile.EQUIPMENTS)
                // .Get<EquipmentsResponse>(API.EQUIPMENTS + "?source=0") // Listen to different action to get smaller data set for each type
                .Subscribe(OnGetEquipments, OnError);
        }

        private void OnError(Exception obj)
        {
            Debug.LogError(obj);
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftEquipmentsFailed());
        }

        private void OnGetEquipments(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.equipments);
            UpdateInboxCache(response.data.equipments);

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftEquipmentsSucceed());
        }

        private void UpdateInGameCache(EquipmentResponse[] equipments)
        {
            if (equipments.Length == 0) return;
            _inGameEquipmentsCache.Clear();
            foreach (var equipment in equipments)
            {
                if (equipment.inGameStatus != (int)EEquipmentStatus.InGame) continue;
                _inGameEquipmentsCache.Add(equipment);
            }

            _inGameEvent.RaiseEvent(_inGameEquipmentsCache);
        }

        private void UpdateInboxCache(EquipmentResponse[] equipments)
        {
            if (equipments.Length == 0) return;
            _inBoxEquipmentsCache.Clear();
            foreach (var equipment in equipments)
            {
                if (equipment.inGameStatus != (int)EEquipmentStatus.InBox) continue;
                _inBoxEquipmentsCache.Add(equipment);
            }

            _inBoxEvent.RaiseEvent(_inBoxEquipmentsCache);
        }
    }
}