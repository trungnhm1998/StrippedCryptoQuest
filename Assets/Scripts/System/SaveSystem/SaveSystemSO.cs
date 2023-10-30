using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [CreateAssetMenu(menuName = "Crypto Quest/SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : ScriptableObject, ISaveSystem
    {
        [SerializeField] protected SaveManagerSO _saveManagerSO;
        [SerializeField] private SaveData _saveData = new();
        public SaveData SaveData => _saveData;

        private List<ISaveObject> _objects = new();

        public string PlayerName
        {
            get => _saveData.PlayerName;
            set => _saveData.PlayerName = value;
        }

        private bool _isSceneLoading = false;

        private void OnEnable()
        {
        }

        private string Save()
        {
            var jsonSaveData = JsonConvert.SerializeObject(_saveData);
            Debug.Log("Save: " + jsonSaveData);
            return jsonSaveData;
        }

        private bool Load(string json)
        {
            if (string.IsNullOrEmpty(json)) return false;
            Debug.Log("Load Save: " + json);
            _saveData = JsonConvert.DeserializeObject<SaveData>(json);
            return true;
        }

        public bool SaveGame()
        {
            var saveData = Save();
            return !string.IsNullOrEmpty(saveData) && _saveManagerSO.Save(saveData);
        }

        public bool LoadSaveGame() => Load(_saveManagerSO.Load());

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

        public void OnSceneLoaded()
        {
            if(_isSceneLoading)
            {
                foreach (var obj in _objects)
                {
                    LoadObject(obj);
                }
                _isSceneLoading = false;
            }
            else
            {
                // walkaround for trigger IsLoaded() when not load from save file
                foreach (var obj in _objects)
                {
                    if (obj != null) obj.FromJson(null);
                }
            }
            
        }

        public bool SaveScene(SceneScriptableObject sceneSO)
        {
            if (!IsLoadingSaveGame())
            {
                _saveData.LastExploreScene = JsonUtility.ToJson(sceneSO);
                return SaveGame();
            }
            return false;
        }

        public bool LoadScene(ref SceneScriptableObject sceneSO)
        {
            if (IsLoadingSaveGame() || string.IsNullOrEmpty(_saveData.LastExploreScene)) return false;
            sceneSO = CreateInstance<SceneScriptableObject>();
            JsonUtility.FromJsonOverwrite(_saveData.LastExploreScene, sceneSO);
            _objects.Clear();
            _isSceneLoading = true;
            return true;
        }

        public bool LoadObject(ISaveObject jObject)
        {
            try
            {
                if (jObject != null && !jObject.IsLoaded())
                {
                    foreach (var data in _saveData.objects)
                    {
                        if (data != null && data.Value != null && data.Key == jObject.Key)
                        {
                            return jObject.FromJson(data.Value);
                        }
                    }
                    return jObject.FromJson("");
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return false;
        }

        public bool SaveObject(ISaveObject jObject)
        {
            if (IsLoadingSaveGame() || jObject == null || !jObject.IsLoaded())
            {
                return false;
            }
            try
            {
                foreach (var data in _saveData.objects)
                {
                    if (data.Key == jObject.Key)
                    {
                        _saveData.objects.Remove(data);
                        break;
                    }
                }
                _saveData.objects.Add(new KeyValue(jObject.Key, jObject.ToJson()));
                return SaveGame();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return false;
        }

        public bool IsLoadingSaveGame()
        {
            return _isSceneLoading;
        }

        public bool RegisterObject(ISaveObject obj)
        {
            if (!_objects.Contains(obj))
            {
                _objects.Add(obj);
                return true;
            }
            return false;
        }

        #region Editor Tools

#if UNITY_EDITOR
        public void Editor_ClearSave()
        {
            _saveData = new SaveData();
            SaveGame();
        }

        public void Editor_OpenSaveFolder()
        {
            Application.OpenURL(Application.persistentDataPath);
        }
#endif

        #endregion
    }
}
