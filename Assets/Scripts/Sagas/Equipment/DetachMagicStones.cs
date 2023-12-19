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
    public class DetachBody
    {
        public string id;
        public List<int> stones;
    }

    public class DetachMagicStones : SagaBase<DetachStones>
    {
        protected override void HandleAction(DetachStones ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            Debug.Log($"<color=white>@@@@@@ id={ctx.EquipmentID} -- stones={ctx.StoneIDs[0]}</color>");
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(new DetachBody()
                {
                    id = ctx.EquipmentID.ToString(),
                    stones = ctx.StoneIDs
                })
                .Post<EquipmentsResponse>(EquipmentAPI.DETACH_MAGIC_STONE)
                .Subscribe(ProcessResponse, OnError, OnCompleted);
        }

        private void ProcessResponse(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new DetachSucceeded());
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::DetachMagicStones::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}