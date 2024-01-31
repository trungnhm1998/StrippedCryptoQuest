using System;
using System.Net;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Ranch.Object;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Ranch.Sagas
{
    public class UpgradeBeastSaga : SagaBase<RequestUpgradeBeast>
    {
        protected override void HandleAction(RequestUpgradeBeast ctx)
        {
            var body = new UpgradeBeastRequest
            {
                BeforeLevel = ctx.Beast.Level,
                Beast = new UpgradeBeastRequest.BeastUpgrade
                {
                    Level = ctx.LevelToUpgrade,
                    BeastId = ctx.Beast.BeastId,
                    Id = ctx.Beast.Id,
                }
            };
            Debug.Log($"UpgradeBeast:: Requesting Upgrade Beast: {JsonConvert.SerializeObject(body)}");

            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .WithBody(body)
                .Post<UpgradeResponse>(BeastAPI.UPGRADE)
                .Subscribe(OnNext, OnError);
        }

        private void OnNext(UpgradeResponse response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Debug.Log($"UpgradeBeast:: Load Data Succeed: {JsonConvert.SerializeObject(response.data)}");
            ActionDispatcher.Dispatch(new ShowLoading(false));

            ActionDispatcher.Dispatch(new BeastUpgradeSucceed());
        }

        private void OnError(Exception exception)
        {
            Debug.Log($"UpgradeBeast:: Load Data Failed: {exception.Message}!");
            ActionDispatcher.Dispatch(new ShowLoading(false));

            ActionDispatcher.Dispatch(new BeastUpgradeFailed());
        }
    }
}