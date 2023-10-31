using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.DimensionalBox.Events;
using CryptoQuest.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.UI.Core;
using UniRx;
using UnityEngine;

namespace CryptoQuest.DimensionalBox.Sagas
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
                _inGameEvent.RaiseEvent(_inGameEquipmentsCache);
                _inBoxEvent.RaiseEvent(_inBoxEquipmentsCache);
                return;
            }

            LoadingController.OnEnableLoadingPanel?.Invoke();
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<EquipmentsResponse>(API.EQUIPMENTS)
                // .Get<EquipmentsResponse>(API.EQUIPMENTS + "?source=0") // Listen to different action to get smaller data set for each type
                .Subscribe(OnGetEquipments, OnError);
        }

        private void OnError(Exception obj)
        {
            LoadingController.OnDisableLoadingPanel?.Invoke();
            Debug.LogError(obj);
        }

        private void OnGetEquipments(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            UpdateInGameCache(response.data.equipments);
            UpdateInboxCache(response.data.equipments);
            
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }

        private void UpdateInGameCache(Equipments[] equipments)
        {
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