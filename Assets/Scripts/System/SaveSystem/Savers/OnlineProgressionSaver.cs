using System;
using CryptoQuest.API;
using CryptoQuest.Networking;
using IndiGames.Core.Common;
using Newtonsoft.Json;
using UnityEngine;
using UniRx;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public interface ISaveHandler
    {
        SaveSystemSO SaveSystem { get; }
        void Save();
    }

    public class OnlineProgressionSaver : MonoBehaviour, ISaveHandler
    {
        [SerializeField] private Credentials _credentials;
        [SerializeField] private SaveSystemSO _saveSystem;
        public SaveSystemSO SaveSystem => _saveSystem;

        [SerializeReference, SubclassSelector] private ISaver[] _progressionSavers;

        [Header("Raise event")]
        [SerializeField] private VoidEventChannelSO _saveFinishedEventChannel;

        private bool _isUploading;
        private IRestClient _restClient;

        private void Awake()
        {
            foreach (var saver in _progressionSavers)
            {
                saver.Init(this);
            }
        }

        private void OnEnable()
        {
            foreach (var saver in _progressionSavers)
            {
                saver.RegistEvents();
            }
        }

        private void OnDisable()
        {
            foreach (var saver in _progressionSavers)
            {
                saver.UnregistEvents();
            }
        }

        public void Save()
        {
            // Uploading might be fail but we still need to save the data locally.
            _saveSystem.Save();
            Debug.Log($"OnlineProgressionSaver::Save - Saved locally! {_isUploading}");

            if (!_credentials.IsLoggedIn() || _isUploading) return;
            _isUploading = true;

            _restClient ??= ServiceProvider.GetService<IRestClient>();
            if (_restClient == null) return;

            _restClient.WithBody(new SaveDataBody() { GameData = _saveSystem.SaveData })
                .Post<SaveDataResult>(Accounts.USER_SAVE_DATA)
                .Subscribe(OnDataSaved, OnError);
        }

        private void OnDataSaved(SaveDataResult _)
        {
            OnUpdateFinished();
            Debug.Log("OnlineProgressionSaver::Save - Uploading succeed!");
        }

        private void OnError(Exception _)
        {
            OnUpdateFinished();
            Debug.Log("OnlineProgressionSaver::Save - Uploading failed!");
        }

        private void OnUpdateFinished()
        {
            _saveFinishedEventChannel.RaiseEvent();
            _isUploading = false;
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
            [JsonProperty("code")]
            public int Code;
        }
    }
}