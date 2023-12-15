using CryptoQuest.Bridge;
using CryptoQuest.Networking;
using CryptoQuest.SaveSystem;
using CryptoQuest.System;
using CryptoQuest.API;
using Newtonsoft.Json;
using System;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Events
{
    public class ApplicationEventListener : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private float _saveInterval = 10f;

        private void Start()
        {
            ApplicationEventHandler.RegisterOnBeforeUnloadEventCallback(gameObject.name, nameof(OnBeforeUnloaded));
            ApplicationEventHandler.RegisterOnFocusChangedEventCallback(gameObject.name, nameof(OnFocusChanged));
        }

        private void OnApplicationFocus(bool focus)
        {
            OnFocusChanged(focus ? 1 : 0);
        }

        private void OnApplicationQuit()
        {
            OnBeforeUnloaded();
        }

        private void OnBeforeUnloaded()
        {
            UploadGameSave();
        }

        private void OnFocusChanged(int hasFocus)
        {
            if (hasFocus == 0)
            {
                UploadGameSave();
            }
        }

        #region Upload Game Save
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

        private DateTime _lastUploadTime = DateTime.Now;
        private bool _isUploading = false;

        private bool IsLoggedIn()
        {
            var credential = ServiceProvider.GetService<Credentials>();
            return credential != null && credential.IsLoggedIn();
        }

        private void UploadGameSave()
        {
            if (!IsLoggedIn() || _isUploading) return;
            Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading profile...");

            var timeToCheck = _lastUploadTime.AddSeconds(_saveInterval);
            if (DateTime.Compare(timeToCheck, DateTime.Now) > 0)
            {
                Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading aborted!");
                return;
            }

            _isUploading = true;
            var restClient = ServiceProvider.GetService<IRestClient>();
            if (restClient != null)
            {
                restClient.WithBody(new SaveDataBody() { GameData = _saveSystem.SaveData })
                    .Post<SaveDataResult>(Accounts.USER_SAVE_DATA)
                    .Subscribe(OnDataSaved, OnError, OnCompleted);
            }
        }

        private void OnUpdateFinished()
        {
            _isUploading = false;
            _lastUploadTime = DateTime.Now;
        }

        private void OnDataSaved(SaveDataResult _)
        {
            OnUpdateFinished();
            Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading succeed!");
        }

        private void OnError(Exception _)
        {
            OnUpdateFinished();
            Debug.Log("ApplicationEventListener::UploadGameSave() - Uploading failed!");
        }

        private void OnCompleted() { }
        #endregion
    }
}
