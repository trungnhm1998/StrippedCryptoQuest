using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class AttachBody
    {
        public string id;
        public List<int> stones;
    }

    public class AttachMagicStones : SagaBase<AttachStones>
    {
        private int _equipmentID;
        private List<int> _stoneIDs;

        protected override void HandleAction(AttachStones ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new AttachBody()
                {
                    id = ctx.EquipmentID.ToString(),
                    stones = ctx.StoneIDs
                })
                .Post<EquipmentsResponse>(EquipmentAPI.ATTACH_MAGIC_STONE)
                .Subscribe(ProcessResponse, OnError, OnCompleted);

            _equipmentID = ctx.EquipmentID;
            _stoneIDs = ctx.StoneIDs;
        }

        private void ProcessResponse(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new AttachSucceeded()
            {
                EquipmentID = _equipmentID,
                StoneIDs = _stoneIDs
            });

            foreach (var equipmentResponse in response.data.equipments)
            {
                if (_equipmentID != equipmentResponse.id || equipmentResponse.attachId == 0) continue;
                ActionDispatcher.Dispatch(new ApplyStonePassiveRequest()
                {
                    EquipmentID = _equipmentID,
                    StoneIDs = _stoneIDs,
                    CharacterID = equipmentResponse.attachId
                });
            }
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::AttachMagicStones::Error</color>:: {error}");
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}