using System;
using System.Collections;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.API;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem.Loaders;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.SaveSystem.Sagas
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
        [SerializeField] private SaveSystemSO _saveSystem;

        private void Awake()
        {
            ServiceProvider.Provide<ISaveSystem>(_saveSystem);
            _saveSystem.Load(); // this will load from storage
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
            if (response.data != null && response.data.ids.Length > 0)
            {
                var restClient = ServiceProvider.GetService<IRestClient>();
                restClient
                    .Get<SaveDataResponse>(Accounts.USER_SAVE_DATA + "/" + response.data.ids[0])
                    .Subscribe(LoadIntoSaveSystem, OnError, OnComplete);
            }
            else
            {
                LoadIntoSaveSystem(null);
                OnComplete();
            }
        }

        private void LoadIntoSaveSystem(SaveDataResponse res)
        {
            Debug.Log("LoadIntoSaveSystem: " + JsonConvert.SerializeObject(res));
            _saveSystem.SaveData = LoadSaveFromResponse(res);
            _saveSystem.Save();
        }

        private SaveData LoadSaveFromResponse(SaveDataResponse res)
        {
            var credential = ServiceProvider.GetService<Credentials>();
            if (res == null || res.game_data == null)
            {
                // This is new account, create new save file
                return new SaveData
                {
                    UUID = credential.Profile.user.email
                };
            }

            // If not same user, or server version is newer, use server version
            if (string.IsNullOrEmpty(res.game_data.UUID)
                || res.game_data.UUID != _saveSystem.SaveData.UUID
                || res.game_data.SavedTime.CompareTo(_saveSystem.SaveData.SavedTime) > 0)
            {
                return new SaveData()
                {
                    UUID = credential.Profile.user.email,
                    PlayerName = res.game_data.PlayerName,
                    Objects = res.game_data.Objects
                };
            }

            // local save
            return _saveSystem.SaveData;
        }

        private void OnError(Exception obj) => Debug.LogException(obj);

        private void OnComplete()
        {
            StartCoroutine(CoLoadProgression());
        }

        private IEnumerator CoLoadProgression()
        {
            var loaders = GetComponents<ILoader>();
            foreach (var loader in loaders) yield return loader.Load(_saveSystem); // adjust loader order on inspector
            ActionDispatcher.Dispatch(new GetProfileSucceed());
        }
    }
}