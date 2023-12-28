using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class RequestUpgradeStoneSaga : SagaBase<RequestUpgradeStone>
    {
        private IRestClient _restClient;
        private RequestUpgradeStone _requestContext;
        private const string UPGRADE_STONE_API = "crypto/magicstone/upgrade";

        protected override void HandleAction(RequestUpgradeStone ctx)
        {
            _requestContext = ctx;
            ActionDispatcher.Dispatch(new ShowLoading());
            _restClient ??= ServiceProvider.GetService<IRestClient>();

            var requestBody = new RequestUpgradeBody()
            {
                Ids = ctx.Stones.Select(stone => stone.ID).ToList()
            };
            
            _restClient.WithBody(requestBody)
                .Post<StoneUpgradeResponse>(UPGRADE_STONE_API)
                .Subscribe(OnUpgradeStoneFinish, OnUpgradeStoneFailed);
        }

        private void OnUpgradeStoneFinish(StoneUpgradeResponse response)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new UpgradeStoneResponsed(response));
            ActionDispatcher.Dispatch(new RemoveAfterUpgradeStone(_requestContext.Stones));
        }

        private void OnUpgradeStoneFailed(Exception obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new RequestUpgradeStoneFailed());
        }

        [Serializable]
        public struct RequestUpgradeBody
        {
            [JsonProperty("ids")]
            public List<int> Ids;
        }
    }
}