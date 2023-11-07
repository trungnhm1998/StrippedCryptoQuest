using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.System;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UniRx;
using CryptoQuest.Networking;
using CryptoQuest.ChangeClass.View;
using Newtonsoft.Json;

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
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(new Body { BaseUnitId1 = firstClassMaterial.Class.Id, BaseUnitId2 = lastClassMaterial.Class.Id })
                .Post<PreviewCharacterData>(ChangeClassAPI.PREVIEW_NEW_CHARACTER)
                .Subscribe(PreviewNewCharacter, OnGetNewClassFailed, OnGetNewClassSuccess);
        }

        private void PreviewNewCharacter(PreviewCharacterData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Data = response.data;
            IsFinishFetchData = true;
        }

        private void OnGetNewClassFailed(Exception obj)
        {
            Debug.Log($"Preview New Character::Load failed : {obj.Message}");
            IsFinishFetchData = true;
        }

        private void OnGetNewClassSuccess()
        {
            Debug.Log($"Preview New Character::Load Success");
        }
    }
}