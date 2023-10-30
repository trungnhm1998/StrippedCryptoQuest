using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SaveSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [CreateAssetMenu(menuName = "Crypto Quest/SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : ScriptableObject, ISaveSystem
    {
        [SerializeField] protected SaveManagerSO _saveManagerSO;
        [SerializeField] private SaveData _saveData = new();

        private List<ISaveObject> _objects = new();

        public string PlayerName
        {
            get => _saveData.player;
            set => _saveData.player = value;
        }

        private bool _isSceneLoading = false;

        private string Save()
        {
            if (!string.IsNullOrEmpty(PlayerName))
            {
                var saveData = JsonUtility.ToJson(_saveData);
                Debug.Log("Save: " + saveData);
                return saveData;
            }
            return null;
        }

        private bool Load(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                Debug.Log("Load Save: " + json);
                JsonUtility.FromJsonOverwrite(json, _saveData);
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

        public void OnSceneLoaded()
        {
            foreach(var obj in _objects)
            {
                LoadObject(obj);
            }
            _isSceneLoading = false;
        }

        public bool SaveScene(SceneScriptableObject sceneSO)
        {
            if (!IsLoadingSaveGame())
            {
                _saveData.scene = JsonUtility.ToJson(sceneSO);
                return SaveGame();
            }
            return false;
        }

        public bool LoadScene(ref SceneScriptableObject sceneSO)
        {
            if (!IsLoadingSaveGame() && !string.IsNullOrEmpty(_saveData.scene) && sceneSO != null)
            {
                sceneSO = SceneScriptableObject.CreateInstance<SceneScriptableObject>();
                JsonUtility.FromJsonOverwrite(_saveData.scene, sceneSO);
                _isSceneLoading = true;
                return true;
            }
            return false;
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
