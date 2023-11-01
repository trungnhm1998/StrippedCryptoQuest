using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Core;
using CryptoQuest.DimensionalBox.Objects;
using CryptoQuest.Events;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Core;
using UniRx;
using UnityEngine;

namespace CryptoQuest.DimensionalBox.Sagas
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
                .Put<TransferResponse>(API.PUT_EQUIPMENTS_TO_GAME, body)
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
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }
        private void OnCompleted() { }
    }
}