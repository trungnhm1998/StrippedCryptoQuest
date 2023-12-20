﻿using System;
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
        }

        private void ProcessResponse(EquipmentsResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new AttachSucceeded());
        }

        private void OnError(Exception error)
        {
            Debug.Log($"<color=white>Saga::AttachMagicStones::Error</color>:: {error}");
            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }

        private void OnCompleted()
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}