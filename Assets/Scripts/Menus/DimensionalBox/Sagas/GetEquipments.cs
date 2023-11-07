using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Battle.Components;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Networking;
using CryptoQuest.Networking.API;
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
        private IPartyController _partyManager;

        protected override void HandleAction(GetNftEquipments ctx)
        {
            var isCacheEmpty = _inGameEquipmentsCache.Count == 0 && _inBoxEquipmentsCache.Count == 0;
            /*
             * TODO: Only update cache if dirty or needed
             * Cache would be dirty after a transfer, or when the user first enters the dimensional box
             */
            if (isCacheEmpty == false && ctx.ForceRefresh == false)
            {
                UpdateInGameCache(_inGameEquipmentsCache.ToArray());
                ActionDispatcher.Dispatch(new GetNftEquipmentsSucceed());
                return;
            }

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
            _partyManager ??= ServiceProvider.GetService<IPartyController>();
            _inGameEquipmentsCache.Clear();
            foreach (var equipment in equipments)
            {
                if (equipment.inGameStatus != (int)EEquipmentStatus.InGame) continue;
                equipment.isEquipped = IsEquipping(equipment);
                _inGameEquipmentsCache.Add(equipment);
            }

            _inGameEvent.RaiseEvent(_inGameEquipmentsCache);
        }

        private bool IsEquipping(EquipmentResponse equipmentResponse)
        {
            foreach (var slot in _partyManager.Slots)
            {
                if (slot.IsValid() == false) continue;
                slot.HeroBehaviour.TryGetComponent(out EquipmentsController equipmentsController);
                var equipmentsSlots = equipmentsController.Equipments.Slots;
                if (equipmentsSlots.Any(equipmentSlot => equipmentSlot.Equipment.Id == equipmentResponse.id))
                {
                    return true;
                }
            }

            return false;
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