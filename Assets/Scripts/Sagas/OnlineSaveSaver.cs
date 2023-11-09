using System;
using System.Collections;
using System.Net;
using CryptoQuest.Core;
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
    public class OnlineSaveAction : ActionBase { }

    public class OnlineSaveSaver : SagaBase<OnlineSaveAction>
    {
        const float SAVE_INTERVAL_IN_SECOND = 5.0f;

        private SaveSystemSO _saveSystem;
        private bool _isSaveDirty = false;
        private bool _isSavingToBackend = false;

        private void Start()
        {
            _saveSystem = (SaveSystemSO)ServiceProvider.GetService<ISaveSystem>();
            _saveSystem.Saved += OnSaved;
            StartCoroutine(CoSaveGameToBackend());
        }

        private void OnDestroy()
        {
            _saveSystem.Saved -= OnSaved;
        }

        private void OnSaved()
        {
            _isSaveDirty = true;
        }

        private IEnumerator CoSaveGameToBackend()
        {
            while(true)
            {
                if(_isSaveDirty && !_isSavingToBackend)
                {
                    _isSavingToBackend = true;
                    ActionDispatcher.Dispatch(new OnlineSaveAction());
                }
                yield return new WaitForSeconds(SAVE_INTERVAL_IN_SECOND);
            }
        }

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

        protected override void HandleAction(OnlineSaveAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient.WithBody(new SaveDataBody() { GameData = _saveSystem.SaveData})
                .Post<SaveDataResult>(Accounts.USER_SAVE_DATA)
                .Subscribe(Saved, OnError, OnCompleted);
        }

        private void OnCompleted()
        {
        }

        private void OnError(Exception obj)
        {
            Debug.Log(obj.Message);
            _isSavingToBackend = false;
        }

        private void Saved(SaveDataResult result)
        {
            Debug.Log("OnlineSaved: " + result.code);
            _isSavingToBackend = false;
            _isSaveDirty = false;
        }
    }
}