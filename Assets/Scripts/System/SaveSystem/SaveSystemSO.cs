using System;
using System.Collections;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    [CreateAssetMenu(menuName = "Crypto Quest/SaveSystem/SaveSystemSO")]
    public class SaveSystemSO : ScriptableObject, ISaveSystem
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;
        [SerializeField] private SaveData _saveData = new();
        public SaveData SaveData => _saveData;

        public string PlayerName
        {
            get => _saveData.PlayerName;
            set => _saveData.PlayerName = value;
        }

        public bool SaveGame()
        {
            _saveData.SavedTime = DateTime.Now;
            var json = JsonConvert.SerializeObject(_saveData); ;
            return !string.IsNullOrEmpty(json) && _saveManagerSO.Save(json);
        }

        public bool LoadGame()
        {
            var json = _saveManagerSO.Load();
            if (!string.IsNullOrEmpty(json))
            {
                Debug.Log("Load Save: " + json);
                _saveData = JsonConvert.DeserializeObject<SaveData>(json);
                return true;
            }
            return false;
        }

        public IEnumerator CoLoadObject(ISaveObject jObject, Action<bool> callback = null)
        {
            if (jObject != null)
            {
                foreach (var data in _saveData.Objects)
                {
                    if (data != null && data.Value != null && data.Key == jObject.Key)
                    {
                        yield return jObject.CoFromJson(data.Value, callback);
                        yield break;
                    }
                }
            }
            if (callback != null) { callback(false); }
            yield break;
        }

        public bool SaveObject(ISaveObject jObject)
        {
            try
            {
                if (jObject != null)
                {
                    foreach (var data in _saveData.Objects)
                    {
                        if (data.Key == jObject.Key)
                        {
                            _saveData.Objects.Remove(data);
                            break;
                        }
                    }
                    _saveData.Objects.Add(new KeyValue(jObject.Key, jObject.ToJson()));
                    return SaveGame();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
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
