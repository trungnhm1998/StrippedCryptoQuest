using System;
using System.Collections;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using APIChangeClass = CryptoQuest.API.ChangeClass;

namespace CryptoQuest.ChangeClass.API
{
    public class PreviewCharacterAPI : MonoBehaviour
    {
        [Serializable]
        public struct Body
        {
            [JsonProperty("baseUnitId1")]
            public string BaseUnitId1;
            [JsonProperty("baseUnitId2")]
            public string BaseUnitId2;
        }

        public struct ClassBerserkerBody
        {
            [JsonProperty("baseUnitId1")]
            public string BaseUnitId;
        }

        public bool IsFinishFetchData { get; private set; }

        public PreviewCharacter Data { get; private set; }

        public IEnumerator CoPreviewNewClass(UICharacter firstClassMaterial, UICharacter lastClassMaterial)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(new Body { BaseUnitId1 = firstClassMaterial.Class.Id.ToString(), BaseUnitId2 = lastClassMaterial.Class.Id.ToString() })
                .Post<PreviewCharacterData>(APIChangeClass.PREVIEW_NEW_CHARACTER)
                .ToYieldInstruction();
            yield return op;
            
            Data = op.Result.data;
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        public IEnumerator CoPreviewClassBerserker(UICharacter characterData)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(new ClassBerserkerBody { BaseUnitId = characterData.Class.Id.ToString() })
                .Post<PreviewCharacterData>(APIChangeClass.PREVIEW_CLASS_BERSERKER)
                .ToYieldInstruction();
            yield return op;
            
            Data = op.Result.data;
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}