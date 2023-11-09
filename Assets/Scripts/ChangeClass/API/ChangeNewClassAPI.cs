using CryptoQuest.System;
using System;
using System.Net;
using UnityEngine;
using UniRx;
using CryptoQuest.Networking;
using CryptoQuest.ChangeClass.View;
using Newtonsoft.Json;
using CryptoQuest.UI.Actions;
using CryptoQuest.Core;
using CryptoQuest.Sagas;
using System.Collections;

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

        public NewCharacter Data { get; private set; }
        public bool IsFinishFetchData { get; private set; }
        private IRestClient _restAPINetworkController;
        private Body _requestBody;

        protected override void HandleAction(GetNewNftClass ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .WithBody(_requestBody)
                .Post<ChangeClassResponseData>(ChangeClassAPI.CHANGE_NEW_CLASS)
                .Subscribe(OnChangeClass, OnChangeClassFailed, OnChangeClassSuccess);
        }

        public void ChangeNewClassData(UICharacter firstClassMaterial, UICharacter lastClassMaterial, UIOccupation occupation)
        {
            _requestBody = new Body
            {
                BaseUnitId1 = firstClassMaterial.Class.id.ToString(),
                BaseUnitId2 = lastClassMaterial.Class.id.ToString(),
                ChangeClassMaterials = new ChangeClassMaterials(occupation.Class.ItemMaterialId.ToString(), occupation.Class.MaterialQuantity)
            };
            ActionDispatcher.Dispatch(new GetNewNftClass { ForceRefresh = true });
        }

        private void OnChangeClass(ChangeClassResponseData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            Data = response.data.newCharacter;
            IsFinishFetchData = true;
        }

        private void OnChangeClassFailed(Exception obj)
        {
            Debug.Log($"ChangeClass:: Load Data Failed: {obj.Message}!");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftClassesFailed());
            IsFinishFetchData = true;
        }

        private void OnChangeClassSuccess()
        {
            Debug.Log($"ChangeClass:: Load Data Success!");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetNftClassesSucceed());
        }
    }
}