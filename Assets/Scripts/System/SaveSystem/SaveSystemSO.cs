using System;
using IndiGames.Core.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    /// <summary>
    /// Wrapper around the PlayerPrefs for network and local saving
    /// </summary>
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private string _version = "0.0.1";
        public string Version => _version;

        [SerializeField] private SaveManagerSO _saveManagerSO;
        [SerializeField] private SaveData _saveData = new();

        private void OnEnable()
        {
            ServiceProvider.Provide(this);
        }

        public SaveData SaveData
        {
            get => _saveData;
            set => _saveData = value;
        }

        public string PlayerName
        {
            get => _saveData.PlayerName;
            set => _saveData.PlayerName = value;
        }

        /// <summary>
        /// Make sure the safe file are latest with current date
        /// </summary>
        /// <returns>true if save file exists</returns>
        public bool Save()
        {
            _saveData.SavedTime = DateTime.Now.ToString();
            _saveData.Version = _version;
            var json = JsonConvert.SerializeObject(_saveData);
            var result = !string.IsNullOrEmpty(json) && _saveManagerSO.Save(json);
            Debug.Log($"Saving[{result}]: {json}");
            return result;
        }

        public bool Load()
        {
            if (!_saveManagerSO.Load(out var json)) return false;
            _saveData = JsonConvert.DeserializeObject<SaveData>(json);
            return _saveData != null && _version == _saveData.Version;
        }

        public string this[string key]
        {
            get => _saveData[key];
            set => _saveData[key] = value;
        }
    }
}