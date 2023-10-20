using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SaveSystem;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "Crypto Quest/SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : SerializableScriptableObject, ISaveSystem
    {
        [SerializeField] protected SaveManagerSO _saveManagerSO;
        [SerializeField] protected VoidEventChannelSO _sceneLoadedEvent;

        [SerializeField] private string _playerName;
        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        [SerializeField] private string _scene;

        [Serializable]
        public class KeyValue
        {
            [SerializeField] public string Key;

            [SerializeField] public string Value;

            public KeyValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }

        [SerializeField] protected List<KeyValue> _saveDatas;

        private bool isSceneLoading = false;

        private string Save()
        {
            if(!string.IsNullOrEmpty(PlayerName))
            {
                var saveData = JsonUtility.ToJson(this);
                Debug.Log("Save: " + saveData);
                return saveData;
            }
            return null;            
        }

        private bool Load(string json)
        {
            if(!string.IsNullOrEmpty(json))
            {
                Debug.Log("Load Save: " + json);
                JsonUtility.FromJsonOverwrite(json, this);
                return true;
            }
            return false;
        }

        public bool SaveGame()
        {
            var saveData = Save();
            if (string.IsNullOrEmpty(saveData)) return false;            
            return _saveManagerSO.Save(saveData);
        }

        public bool LoadSaveGame()
        {
            return Load(_saveManagerSO.Load());
        }

        public virtual async Task<bool> SaveGameAsync()
        {
            var saveData = Save();
            if (string.IsNullOrEmpty(saveData)) return false;
            return await _saveManagerSO.SaveAsync(saveData);
        }

        public virtual async Task<bool> LoadSaveGameAsync()
        {        
            return Load(await _saveManagerSO.LoadAsync());
        }

        private void OnSceneLoaded()
        {
            if(isSceneLoading)
            {
                _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
                isSceneLoading = false;
            }
        }

        public bool SaveScene(SceneScriptableObject sceneSO)
        {
            _scene = JsonUtility.ToJson(sceneSO);            
            return SaveGame(); ;
        }

        public bool LoadScene(SceneScriptableObject sceneSO)
        {
            if(!string.IsNullOrEmpty(_scene) && sceneSO != null)
            {
                JsonUtility.FromJsonOverwrite(_scene, sceneSO);
                _sceneLoadedEvent.EventRaised += OnSceneLoaded;
                isSceneLoading = true;
                return true;
            }
            return false;
        }

        public bool LoadObject(IJsonSerializable jObject)
        {
            foreach(var data in _saveDatas)
            {
                if (data != null && data.Value != null && data.Key == jObject.Key)
                {
                    return jObject.FromJson(data.Value);
                }
            }                       
            return false;
        }

        public bool SaveObject(IJsonSerializable jObject)
        {
            if(jObject != null)
            {
                foreach (var data in _saveDatas)
                {
                    if(data.Key == jObject.Key)
                    {
                        return false;
                    }
                }
                _saveDatas.Add(new KeyValue(jObject.Key, jObject.ToJson()));
                return SaveGame();
            }
            return false;
        }
    }
}
