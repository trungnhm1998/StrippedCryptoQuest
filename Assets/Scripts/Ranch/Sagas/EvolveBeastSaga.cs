using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    [Serializable]
    public class EvolveResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public BeastResponseData data;

        [Serializable]
        public class BeastResponseData
        {
            public int success;
            public BeastResponeEvolveData newBeast;
        }
    }

    [Serializable]
    public class EvolveBeastRequest
    {
        [JsonProperty("baseBeastId1")] public string BaseBeast;

        [JsonProperty("baseBeastId2")] public string MaterialBeast;
    }

    public class EvolveBeastSaga : SagaBase<RequestEvolveBeast>
    {
        private RequestEvolveBeast _requestContext;

        protected override void HandleAction(RequestEvolveBeast ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            _requestContext = ctx;

            var body = new EvolveBeastRequest
            {
                BaseBeast = ctx.Base.Id.ToString(),
                MaterialBeast = ctx.Material.Id.ToString()
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<EvolveResponse>(BeastAPI.EVOLVE)
                .Subscribe(HandleRequestSuccess, HandleRequestFailed);
        }

        private void HandleRequestSuccess(EvolveResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"EvolveBeast:: Load Data Success!");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new EvolveRequestSuccess());
            ActionDispatcher.Dispatch(new BeastEvolveRespond(response, _requestContext));
            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }

        private void HandleRequestFailed(Exception exception)
        {
            Debug.Log($"EvolveBeast:: Load Data Failed: {exception.Message}!");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new EvolveRequestFailed());
        }
    }
}