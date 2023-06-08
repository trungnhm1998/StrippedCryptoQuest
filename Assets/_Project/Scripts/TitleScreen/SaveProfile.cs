using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryptoQuestClient
{
    public class SaveProfile
    {
        #region Variables

        [Header("References")]
        [HideInInspector] public ProfileScriptableObject _profileSO;
        [HideInInspector] public string _fileName;

        [HideInInspector] public string PlayerName;
        [HideInInspector] public bool ExistProfile = false;

        [Tooltip("Check file")]
        private string _saveFile;
        #endregion

        #region Class
        public void SaveData()
        {
            _profileSO.PlayerName = PlayerName;
            string jsonString = JsonUtility.ToJson(_profileSO);
            _saveFile = Application.persistentDataPath + "/" + _fileName;
            File.WriteAllText(_saveFile, jsonString);
        }

        public void LoadData()
        {
            _saveFile = Application.persistentDataPath + "/" + _fileName;
            if (File.Exists(_saveFile))
            {
                string fileContents = File.ReadAllText(_saveFile);
                ExistProfile = true;
            }
        }
        #endregion
    }
}