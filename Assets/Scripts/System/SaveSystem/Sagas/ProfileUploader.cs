using System;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class UploadProfileAction : ActionBase { }

    public class ProfileUploader : SagaBase<UploadProfileAction>
    {
        [SerializeField] private SaveSystemSO _saveSystem;

        private bool _isUploading = false;

        [Serializable]
        public class SaveDataBody
        {
            [JsonProperty("game_data")]
            public SaveData GameData;
        }

        [Serializable]
        public class SaveDataResult
        {
            public int code;
        }

        protected override void HandleAction(UploadProfileAction ctx)
        {
            if (_isUploading) return;
            _isUploading = true;

            var restClient = ServiceProvider.GetService<IRestClient>();
            if (restClient != null)
            {
                restClient.WithBody(new SaveDataBody() { GameData = _saveSystem.SaveData })
                    .Post<SaveDataResult>(Accounts.USER_SAVE_DATA)
                    .Subscribe(OnDataSaved, OnError, OnCompleted);
            }
            else
            {
                _isUploading = false;
            }
        }

        private void OnDataSaved(SaveDataResult _) => _isUploading = false;
        private void OnError(Exception _) => _isUploading = false;
        private void OnCompleted() { }
    }
}