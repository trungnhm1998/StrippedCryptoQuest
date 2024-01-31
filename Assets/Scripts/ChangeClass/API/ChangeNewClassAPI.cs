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
    public class ChangeNewClassAPI : SagaBase<GetNewNftClass>
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("baseUnitId1")]
            public string BaseUnitId1;

            [JsonProperty("baseUnitId2")]
            public string BaseUnitId2;

            [JsonProperty("materials")]
            public ChangeClassMaterials ChangeClassMaterials;
        }

        [Serializable]
        public class ChangeClassMaterials
        {
            [JsonProperty("materialId")]
            public string MaterialId;

            [JsonProperty("materialNum")]
            public int MaterialNum;

            public ChangeClassMaterials(string id, int quantity)
            {
                MaterialId = id;
                MaterialNum = quantity;
            }
        }

        private Body _requestBody;

        protected override void HandleAction(GetNewNftClass ctx)
        {
            StartCoroutine(CoChangeClass(ctx));
        }

        private IEnumerator CoChangeClass(GetNewNftClass ctx)
        {
            _requestBody = new Body
            {
                BaseUnitId1 = ctx.BaseUnitId1.ToString(),
                BaseUnitId2 = ctx.BaseUnitId2.ToString(),
                ChangeClassMaterials = new ChangeClassMaterials(ctx.Occupation.Class.ItemMaterialId.ToString(),
                    ctx.Occupation.Class.MaterialQuantity)
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(_requestBody)
                .Post<ChangeClassResponseData>(APIChangeClass.CHANGE_NEW_CLASS)
                .ToYieldInstruction();
            yield return op;
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ChangeNewClassDataRespond(op.Result.data.newCharacter));
        }
    }
}