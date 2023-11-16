using CryptoQuest.Core;
using CryptoQuest.System;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UniRx;
using CryptoQuest.Networking;
using CryptoQuest.ChangeClass.View;
using Newtonsoft.Json;
using CryptoQuest.UI.Actions;
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
                .WithBody(new Body { BaseUnitId1 = firstClassMaterial.Class.id.ToString(), BaseUnitId2 = lastClassMaterial.Class.id.ToString() })
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