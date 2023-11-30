using System;
using System.Collections;
using CryptoQuest.Networking;
using CryptoQuest.API;
using CryptoQuest.SaveSystem;
using IndiGames.Core.Common;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class IntervalOnlineProgressionSaver : MonoBehaviour
    {
        [SerializeField] private float _saveInterval = 5.0f;
        [SerializeField] private SaveSystemSO _saveSystem;
        private bool _isSavingToBackend = false;

        private IEnumerator Start()
        {
            yield return CoSaveGameToBackend();
        }

        private IEnumerator CoSaveGameToBackend()
        {
            while (true)
            {
                if (_isSavingToBackend)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }

                _isSavingToBackend = true;
                SaveGameToServer();
                yield return new WaitForSeconds(_saveInterval);
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

        private void SaveGameToServer()
        {
            var restClient = ServiceProvider.GetService<IRestClient>();
            _saveSystem.SaveData.SavedTime = DateTime.Now.ToString();
            restClient.WithBody(new SaveDataBody() { GameData = _saveSystem.SaveData })
                .Post<SaveDataResult>(Accounts.USER_SAVE_DATA)
                .Subscribe(Saved, OnError, OnCompleted);
        }

        private void OnCompleted() => _isSavingToBackend = false;

        private void Saved(SaveDataResult result) => Debug.Log("OnlineSaved: " + result.code);

        private void OnError(Exception obj) => Debug.Log(obj.Message);
    }
}