using System;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    [CreateAssetMenu(menuName = "Crypto Quest/SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : ScriptableObject, ISaveSystem
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;
        [SerializeField] private SaveData _saveData = new();

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
            _saveData.SavedTime = DateTime.Now;
            var json = JsonConvert.SerializeObject(_saveData);
            return !string.IsNullOrEmpty(json) && _saveManagerSO.Save(json);
        }

        public bool Load()
        {
            if (!_saveManagerSO.Load(out var json)) return false;
            _saveData = JsonConvert.DeserializeObject<SaveData>(json);
            return true;
        }

        public bool LoadObject(ISaveObject jObject)
        {
            if (jObject == null) return false;
            return _saveData.TryGetValue(jObject.Key, out var json) && jObject.FromJson(json);
        }

        public bool SaveObject(ISaveObject jObject)
        {
            if (jObject == null) return false;
            _saveData[jObject.Key] = jObject.ToJson();
            Save(); 
            return true;
        }
    }
}