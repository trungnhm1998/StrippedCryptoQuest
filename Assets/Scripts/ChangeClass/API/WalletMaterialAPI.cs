using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using APIChangeClass = CryptoQuest.API.ChangeClass;

namespace CryptoQuest.ChangeClass.API
{
    public class WalletMaterialAPI : MonoBehaviour
    {
        private IRestClient _restAPINetworkController;

        public List<MaterialAPI> Data { get; private set; } = new();
        public bool IsFinishFetchData { get; private set; }

        public void LoadMaterialsFromWallet()
        {
            Data.Clear();
            ActionDispatcher.Dispatch(new ShowLoading());
            IsFinishFetchData = false;
            _restAPINetworkController = ServiceProvider.GetService<IRestClient>();
            _restAPINetworkController
                .Get<MaterialResponseData>(APIChangeClass.LOAD_MATERIAL)
                .Subscribe(OnGetMaterials, OnGetMaterialsFailed, OnGetMaterialsSuccess);
        }

        private void OnGetMaterials(MaterialResponseData response)
        {
            if (response.code != (int)HttpStatusCode.OK) return;
            IsFinishFetchData = true;
            Data = response.data.materials;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnGetMaterialsFailed(Exception obj)
        {
            Debug.Log($"ChangeClass::Load failed : {obj.Message}");
            IsFinishFetchData = true;
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }

        private void OnGetMaterialsSuccess()
        {
            Debug.Log($"ChangeClass::Load Success");
        }
    }
}