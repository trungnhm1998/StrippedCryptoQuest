using System;
using System.Net;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
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

        private IRestClient _restAPINetworkController;
        public bool IsFinishFetchData { get; private set; }

        public PreviewCharacter Data { get; private set; }

        public void LoadDataToPreviewCharacter(UICharacter firstClassMaterial, UICharacter lastClassMaterial)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { BaseUnitId1 = firstClassMaterial.Class.Id.ToString(), BaseUnitId2 = lastClassMaterial.Class.Id.ToString() })
                .Post<PreviewCharacterData>(APIChangeClass.PREVIEW_NEW_CHARACTER)
                .Subscribe(PreviewNewCharacter, OnGetNewClassFailed, OnGetNewClassSuccess);
        }

        private void PreviewNewCharacter(PreviewCharacterData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Data = response.data;
        }

        private void OnGetNewClassFailed(Exception obj)
        {
            Debug.Log($"Preview New Character::Load failed : {obj.Message}");
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnGetNewClassSuccess()
        {
            Debug.Log($"Preview New Character::Load Success");
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}