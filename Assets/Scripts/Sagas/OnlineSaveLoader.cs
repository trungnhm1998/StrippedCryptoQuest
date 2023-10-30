using System;
using System.Net;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Networking.API;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    [Serializable]
    public class UserSaveDataResponse
    {
        [Serializable]
        public class Data
        {
            public int[] ids { get; set; }
        }

        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string uuid { get; set; }
        public int gold { get; set; }
        public int diamond { get; set; }
        public int soul { get; set; }
        public long time { get; set; }
        public Data data { get; set; }
        public int page_size { get; set; }
        public int page { get; set; }
        public int total_page { get; set; }
    }

    [Serializable]
    public class SaveDataResponse
    {
        [JsonProperty("game_data")]
        public SaveData GameData { get; set; }
    }

    public class OnlineSaveLoader : SagaBase<AuthenticateSucceed>
    {
        [SerializeField] private SaveSystemSO _saveSystem;

        private void Awake() => ServiceProvider.Provide<ISaveSystem>(_saveSystem);

        protected override void HandleAction(AuthenticateSucceed ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get(Accounts.USER_SAVE_DATA)
                .Subscribe(LoadSave, OnError, OnCompleted);
            
            restClient
                .Get(Accounts.CHARACTERS)
                .Subscribe(LoadSave, OnError, OnCompleted);
        }

        private void OnCompleted()
        {
        }

        private void OnError(Exception obj)
        {
            Debug.Log(obj.Message);
        }

        private void LoadSave(string s)
        {
            // if (res.code != (int)HttpStatusCode.OK) return;
            // var restClient = ServiceProvider.GetService<IRestClient>();
            // restClient
            //     .Get<SaveDataResponse>(Accounts.USER_SAVE_DATA + "/" + res.data.ids[0])
            //     .Subscribe(LoadIntoSaveSystem);
        }

        private void LoadIntoSaveSystem(SaveDataResponse res)
        {
            if (res.GameData == null) return;
        }
    }
}