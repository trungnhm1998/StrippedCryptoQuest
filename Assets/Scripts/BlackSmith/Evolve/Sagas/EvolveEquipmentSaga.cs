using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    [Serializable]
    public class EvolveResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public EquipmentResponseData data;

        [Serializable]
        public class EquipmentResponseData
        {
            public int success;
            public EquipmentResponse newEquipment;
        }
    }

    [Serializable]
    public class EvolveEquipmentRequest
    {
        [JsonProperty("baseEquipmentId1")]
        public string EquipmentId;
        [JsonProperty("baseEquipmentId2")]
        public string MaterialId;
    }

    public class EvolveEquipmentSaga : SagaBase<EvolveEquipmentAction>
    {
        public static readonly string EvolveEquipmentApi = "crypto/equipments/evolve";

        protected override void HandleAction(EvolveEquipmentAction ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            var body = new EvolveEquipmentRequest
            {
                EquipmentId = ctx.EquipmentId,
                MaterialId = ctx.MaterialId
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<EvolveResponse>(EvolveEquipmentApi)
                .Subscribe(HandleRequestSuccess, HandleRequestFailed);
        }

        private void HandleRequestSuccess(EvolveResponse response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            int evolveStatus = response.data.success;

            switch (evolveStatus)
            {
                case 0:
                    ActionDispatcher.Dispatch(new EvolveEquipmentFailedAction());
                    break;
                case 1:
                    ActionDispatcher.Dispatch(new EvolveEquipmentSuccessAction());
                    break;
                default:
                    Debug.LogError("[EvolveEquipment]:: unknown success status: " + evolveStatus);
                    break;
            }
        }

        private void HandleRequestFailed(Exception exception)
        {
            Debug.LogError("[EvolveEquipment]:: Error: " + exception.Message);
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}
