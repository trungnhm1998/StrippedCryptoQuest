using System;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class UpgradeStonePreviewRequestHandler : SagaBase<UpgradeStonePreviewRequest>
    {
        private IRestClient _restClient;

        public class RequestUpgradePreviewBody
        {
            [JsonProperty("ids")]
            public int[] Ids;
        }

        protected override void HandleAction(UpgradeStonePreviewRequest ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            _restClient ??= ServiceProvider.GetService<IRestClient>();

            var requestBody = new RequestUpgradePreviewBody()
            {
                Ids = ctx.StoneIds.ToArray()
            };
            Debug.Log($"UpgradeStonePreviewRequestHandler::HandleAction: {JsonUtility.ToJson(requestBody)}");

            _restClient.WithBody(requestBody)
                .Post<StoneUpgradePreviewResponse>(MagicStoneAPI.UPGRADE_STONE_PREVIEW)
                .Subscribe(OnPreviewUpgradeStoneFinish, OnPreviewUpgradeStoneFailed);
        }

        private void OnPreviewUpgradeStoneFinish(StoneUpgradePreviewResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new UpgradeStonePreviewResponse(obj));
        }

        private void OnPreviewUpgradeStoneFailed(Exception obj) { }
    }
}