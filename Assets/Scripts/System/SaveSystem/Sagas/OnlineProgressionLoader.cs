using System;
using System.Collections;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem.Loaders;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    [Serializable]
    public class Data
    {
        public int[] ids;
    }

    [Serializable]
    public class UserSaveDataResponse
    {
        public int code;
        public Data data;
    }

    [Serializable]
    public class SaveDataResponse
    {
        public SaveData game_data;
    }

    public class OnlineProgressionLoader : SagaBase<AuthenticateSucceed>
    {
        [SerializeField] private Credentials _credentials;
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeReference, SubclassSelector] private ILoader[] _progressionLoaders;

        private void Awake()
        {
            // this will load from local save
            if (!_saveSystem.Load()) _saveSystem.SaveData = new SaveData();
        }

        protected override void HandleAction(AuthenticateSucceed ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<UserSaveDataResponse>(Accounts.USER_SAVE_DATA)
                .Subscribe(LoadSave, delegate(Exception exception)
                {
                    Debug.LogWarning($"OnlineProgressionLoader::HandleAction {exception}");
                    OnComplete();
                });
        }

        private void LoadSave(UserSaveDataResponse response)
        {
            Debug.Log("LoadSave: " + JsonConvert.SerializeObject(response));
            if (response.code != (int)HttpStatusCode.OK) return;

            if (response.data.ids.Length == 0)
            {
                UseNewSaveOnError(new Exception("No save found"));
                OnComplete();
                return;
            }
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<SaveDataResponse>(Accounts.USER_SAVE_DATA + "/" + response.data.ids[0])
                .Subscribe(LoadIntoSaveSystem, UseNewSaveOnError, OnComplete);
        }

        private void LoadIntoSaveSystem(SaveDataResponse res)
        {
            _saveSystem.SaveData = ValidateSaveDataFromResponse(res);
            _saveSystem.Save();
        }

        private SaveData ValidateSaveDataFromResponse(SaveDataResponse res)
        {
            var saveData = res.game_data; // priority online save first

            // TODO: Violate OCP implement CoR pattern for new type of check
            ClearSaveIfVersionIsDifferent(ref saveData);
            UsingOnlineSaveIfUserAreDifferent(res, ref saveData);
            return saveData;
        }

        private void ClearSaveIfVersionIsDifferent(ref SaveData saveData)
        {
            if (saveData.Version == _saveSystem.Version) return;
            Debug.Log("Version Diff, clearing old save");
            saveData.Objects = new();
        }

        /// <summary>
        /// This one tend to bug if SavedTime is different format and will cause error
        /// when parsing to DateTime and cause user lose all saved data
        /// so I removed it for now, sync date time format when use this
        /// </summary>
        /// <param name="saveData"></param>
        private void UsingLocalSaveIfNewer(ref SaveData saveData)
        {
            if (string.IsNullOrEmpty(saveData.SavedTime) ||
                string.IsNullOrEmpty(_saveSystem.SaveData.SavedTime)) return;
            var onlineSavedTime = DateTime.Parse(saveData.SavedTime);
            var localSavedTime = DateTime.Parse(_saveSystem.SaveData.SavedTime);
            if (onlineSavedTime.CompareTo(localSavedTime) > 0) return;
            Debug.Log("Local Save is newer");
            saveData.Objects = _saveSystem.SaveData.Objects;
            saveData.PlayerName = _saveSystem.SaveData.PlayerName;
        }


        private void UsingOnlineSaveIfUserAreDifferent(SaveDataResponse res, ref SaveData saveData)
        {
            if (!string.IsNullOrEmpty(saveData.UUID) && saveData.UUID == _saveSystem.SaveData.UUID) return;
            Debug.Log("User Diff");
            saveData = res.game_data;
            saveData.UUID = _credentials.UUID;
            // return saveData; // TODO: CoR should early exit while others should return next node to check
        }

        private void UseNewSaveOnError(Exception obj)
        {
            Debug.LogWarning($"OnlineProgressionLoader::UseNewSaveOnError {obj.Message}");
            _saveSystem.SaveData = new SaveData
            {
                UUID = _credentials.UUID
            };
            _saveSystem.Save();
        }

        private void OnComplete()
        {
            Debug.Log("Loading progression...");
            StartCoroutine(CoLoadProgression());
        }

        private IEnumerator CoLoadProgression()
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            foreach (var loader in _progressionLoaders)
            {
                Debug.Log($"OnlineProgressionLoader::CoLoadProgression::{loader.GetType().Name} loading...");
                loader.Load();
                yield return loader.LoadAsync(); // adjust loader order on inspector
            }

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new GetProfileSucceed());
        }

#if UNITY_EDITOR
        [ContextMenu("ForceLoad")]
        public void ForceLoad()
        {
            if (Application.isPlaying == false) return;
            OnComplete();
        }
#endif
    }
}