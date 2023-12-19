using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Popups;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    [Serializable]
    public class EvolveResponse : CommonResponse
    {
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

    public class EvolveEquipmentSaga : SagaBase<RequestEvolveEquipment>
    {
        public static readonly string EvolveEquipmentApi = "crypto/equipments/evolve";

        private RequestEvolveEquipment _requestContext;

        protected override void HandleAction(RequestEvolveEquipment ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            _requestContext = ctx;

            var body = new EvolveEquipmentRequest
            {
                EquipmentId = ctx.Equipment.Id.ToString(),
                MaterialId = ctx.Material.Id.ToString()
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

            ActionDispatcher.Dispatch(new EvolveResponsed(response, _requestContext));
        }

        private void HandleRequestFailed(Exception exception)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
            
            ActionDispatcher.Dispatch(new EvolveRequestFailed());
        }
    }
}
