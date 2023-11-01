﻿using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Core;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class TransferEquipmentsToDimensionalBox : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _transferEquipmentsToDimensionalBoxChannel;

        private void OnEnable()
        {
            _transferEquipmentsToDimensionalBoxChannel.EventRaised += CallAPI;
        }

        private void OnDisable()
        {
            _transferEquipmentsToDimensionalBoxChannel.EventRaised -= CallAPI;
        }

        private void CallAPI(string equipments)
        {
            var body = new Dictionary<string, string>()
            {
                { "ids", equipments }
            };
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Put<TransferResponse>(API.PUT_EQUIPMENTS_TO_DIMENSIONAL_BOX, body)
                // .Get<EquipmentsResponse>(API.EQUIPMENTS + "?source=0") // Listen to different action to get smaller data set for each type
                .Subscribe(OnNext, OnError, OnCompleted);
        }

        private void OnNext(TransferResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            ActionDispatcher.Dispatch(new TransferSucceed());
            ActionDispatcher.Dispatch(new GetNftEquipments { ForceRefresh = true });
        }

        private void OnError(Exception obj)
        {
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }

        private void OnCompleted() { }
    }
}