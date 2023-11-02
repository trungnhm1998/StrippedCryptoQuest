using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Menus.DimensionalBox.Events;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Core;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class GetEquipments : SagaBase<GetNftEquipments>
    {
        [SerializeField] private GetEquipmentsEvent _inGameEvent;
        [SerializeField] private GetEquipmentsEvent _inBoxEvent;

        // TODO: Not good practice to save data in saga
        private readonly List<NftEquipment> _inGameEquipmentsCache = new();
        private readonly List<NftEquipment> _inBoxEquipmentsCache = new();

        protected override void HandleAction(GetNftEquipments ctx)
        {
            var isCacheEmpty = _inGameEquipmentsCache.Count == 0 && _inBoxEquipmentsCache.Count == 0;
            /*
             * TODO: Only update cache if dirty or needed
             * Cache would be dirty after a transfer, or when the user enters the dimensional box
             */
            if (isCacheEmpty == false && ctx.ForceRefresh == false)
            {
                ActionDispatcher.Dispatch(new GetNftEquipmentsSucceed());
                return;
            }

            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithParams(new Dictionary<string, string>() { { "source", $"{((int)ctx.Status).ToString()}" } })
                .Get<EquipmentsResponse>(API.EQUIPMENTS)
                // .Get<EquipmentsResponse>(API.EQUIPMENTS + "?source=0") // Listen to different action to get smaller data set for each type
                .Subscribe(OnGetEquipments, OnError);
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            Debug.LogError(obj);
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

        private void UpdateInGameCache(Equipments[] equipments)
        {
            if (equipments.Length == 0) return;
            _inGameEquipmentsCache.Clear();
            foreach (var equipment in equipments)
            {
                if (equipment.inGameStatus != (int)EDimensionalBoxStatus.InGame) continue;
                _inGameEquipmentsCache.Add(new NftEquipment
                {
                    Id = equipment.id
                });
            }

            _inGameEvent.RaiseEvent(_inGameEquipmentsCache);
        }

        private void UpdateInboxCache(Equipments[] equipments)
        {
            if (equipments.Length == 0) return;
            _inBoxEquipmentsCache.Clear();
            foreach (var equipment in equipments)
            {
                if (equipment.inGameStatus != (int)EDimensionalBoxStatus.InBox) continue;
                _inBoxEquipmentsCache.Add(new NftEquipment
                {
                    Id = equipment.id
                });
            }

            _inBoxEvent.RaiseEvent(_inBoxEquipmentsCache);
        }
    }
}