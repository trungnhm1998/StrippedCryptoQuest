using System;
using System.Collections;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using APIChangeClass = CryptoQuest.API.ChangeClass;

namespace CryptoQuest.ChangeClass.API
{
    public class ChangeNewClassBerserkerAPI : SagaBase<GetNewNftClassBerserker>
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("baseUnitId1")]
            public string BaseUnitId1;
        }

        private Body _requestBody;

        protected override void HandleAction(GetNewNftClassBerserker ctx)
        {
            StartCoroutine(CoChangeClass(ctx));
        }

        private IEnumerator CoChangeClass(GetNewNftClassBerserker ctx)
        {
            _requestBody = new Body
            {
                BaseUnitId1 = ctx.BaseUnitId.ToString()
            };
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(_requestBody)
                .Post<ChangeClassResponseData>(APIChangeClass.CHANGE_NEW_CLASS_BERSERKER)
                .ToYieldInstruction();
            yield return op;

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ChangeNewClassDataResponse(op.Result.data.newCharacter));
        }
    }
}