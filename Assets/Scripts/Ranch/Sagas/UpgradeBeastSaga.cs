using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    [Serializable]
    public class UpgradeResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public UpgradeBeastResponseData data;
    }

    [Serializable]
    public class UpgradeBeastResponseData
    {
        public int success;
        public BeastResponseUpgradeData newBesat;
    }

    [Serializable]
    public class UpgradeBeastRequest
    {
        [JsonProperty("beforeLv")]
        public int beforeLevel;

        [JsonProperty("beast")]
        public BeastUpgrade beast;

        [Serializable]
        public class BeastUpgrade
        {
            [JsonProperty("level")]
            public int level;

            [JsonProperty("beastId")]
            public string beastId;

            [JsonProperty("id")]
            public int id;
        }
    }

    public class UpgradeBeastSaga : SagaBase<RequestUpgradeBeast>
    {
        private RequestUpgradeBeast _requestContext;

        protected override void HandleAction(RequestUpgradeBeast ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());

            _requestContext = ctx;

            var body = new UpgradeBeastRequest
            {
                beforeLevel = ctx.BeforeLevel,
                beast = new UpgradeBeastRequest.BeastUpgrade
                {
                    level = ctx.Beast.Level,
                    beastId = ctx.Beast.Id.ToString(),
                    id = ctx.Beast.Id
                }
            };

            Debug.Log($"UpgradeBeast:: Requesting: {JsonConvert.SerializeObject(body)}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<UpgradeResponse>(BeastAPI.UPGRADE)
                .Subscribe(HandleRequestSuccess, HandleRequestFailed);
        }

        private void HandleRequestSuccess(UpgradeResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"UpgradeBeast:: Load Data Success!");
            ActionDispatcher.Dispatch(new ShowLoading(false));

            ActionDispatcher.Dispatch(new UpgradeResponsed(response, _requestContext));
            ActionDispatcher.Dispatch(new UpgradeSucceed());
            ActionDispatcher.Dispatch(new GetBeasts());
        }

        private void HandleRequestFailed(Exception exception)
        {
            Debug.Log($"UpgradeBeast:: Load Data Failed: {exception.Message}!");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new UpgradeRequestFailed());
            ActionDispatcher.Dispatch(new ServerErrorPopup());
        }
    }
}