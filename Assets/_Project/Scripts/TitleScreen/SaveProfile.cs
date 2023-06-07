using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryptoQuestClient
{
    public class SaveProfile : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Profile _profile;
        [SerializeField] private string _fileNameJson;

        [HideInInspector]
        public string PlayerName;

        [Tooltip("Check file")]
        private string _saveFile;

        public bool ExistProfile = false;
        #endregion

        #region Unity_Method
        void Awake()
        {
            _saveFile = Application.persistentDataPath + "/" + _fileNameJson + ".json";
            LoadData();
        }
        #endregion

        #region Class
        public void SaveData(string playerName)
        {
            SaveProfile saveProfile = new SaveProfile();
            saveProfile.PlayerName = playerName;
            string jsonString = JsonUtility.ToJson(saveProfile);
            string filePath = Application.persistentDataPath + "/" + _fileNameJson + ".json";
            File.WriteAllText(filePath, jsonString);
        }

        public void LoadData()
        {
            if (File.Exists(_saveFile))
            {
                string fileContents = File.ReadAllText(_saveFile);
                _profile = JsonUtility.FromJson<Profile>(fileContents);
                ExistProfile = true;
            }
        }
        #endregion
    }
}