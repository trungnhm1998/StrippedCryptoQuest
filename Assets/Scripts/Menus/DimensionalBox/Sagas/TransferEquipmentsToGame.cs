﻿using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Events;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferEquipmentsToGame : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _transferEquipmentsToGameChannel;

        private void OnEnable()
        {
            _transferEquipmentsToGameChannel.EventRaised += CallAPI;
        }

        private void OnDisable()
        {
            _transferEquipmentsToGameChannel.EventRaised -= CallAPI;
        }

        private void CallAPI(string equipments)
        {
            var body = new Dictionary<string, string>()
            {
                { "ids", equipments }
            };
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Put<TransferResponse>(Profile.PUT_EQUIPMENTS_TO_GAME)
                // .Get<EquipmentsResponse>(API.EQUIPMENTS + "?source=0") // Listen to different action to get smaller data set for each type
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed());
            ActionDispatcher.Dispatch(new GetNftEquipments{ ForceRefresh = true });
        }

        private void OnError(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new TransferFailed());
        }

        private void OnCompleted() { }
    }
}