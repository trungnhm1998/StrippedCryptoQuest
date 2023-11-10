using System;
using System.Collections.Generic;
using System.Net;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Networking;
using CryptoQuest.Networking.API;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
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

    public class OnlineSaveLoader : SagaBase<AuthenticateSucceed>
    {
        [SerializeField] private SaveSystemSO _saveSystem;

        private void Awake() => ServiceProvider.Provide<ISaveSystem>(_saveSystem);

        protected override void HandleAction(AuthenticateSucceed ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<UserSaveDataResponse>(Accounts.USER_SAVE_DATA)
                .Subscribe(LoadSave, OnError, OnCompleted);
        }

        private void OnCompleted()
        {
        }

        private void OnError(Exception obj)
        {
            Debug.LogException(obj);
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
                    .Subscribe(LoadIntoSaveSystem);
            }
            else
            {
                LoadIntoSaveSystem(null);
            }
        }

        private void LoadIntoSaveSystem(SaveDataResponse res)
        {
            Debug.Log("LoadIntoSaveSystem: " + JsonConvert.SerializeObject(res));
            var credential = ServiceProvider.GetService<Credentials>();
            var saveSystem = (SaveSystemSO)ServiceProvider.GetService<ISaveSystem>();
            if (res == null || res.game_data == null)
            {
                // This is new account, create new save file
                saveSystem.SaveData.Uuid = credential.Profile.user.email;
                saveSystem.SaveData.PlayerName = null;
                saveSystem.SaveData.Objects = new();
                saveSystem.SaveGame();
            }
            else
            {
                // If not same user, or server version is newer, use server version
                if (string.IsNullOrEmpty(res.game_data.Uuid) 
                    || res.game_data.Uuid != saveSystem.SaveData.Uuid
                    || res.game_data.SavedTime.CompareTo(saveSystem.SaveData.SavedTime) > 0)
                {
                    saveSystem.SaveData.Uuid = credential.Profile.user.email;
                    saveSystem.SaveData.PlayerName = res.game_data.PlayerName;
                    saveSystem.SaveData.Objects = res.game_data.Objects;
                    saveSystem.SaveGame();
                }
            }
            saveSystem?.LoadGame();
            ActionDispatcher.Dispatch(new GetProfileSucceed());
        }
    }
}