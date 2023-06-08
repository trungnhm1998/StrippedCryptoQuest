using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryptoQuestClient
{
    public static class ProfileManager
    {
        
        public static void WriteData(ProfileScriptableObject _profileSO, string _fileName)
        {
            string jsonString = JsonUtility.ToJson(_profileSO);
            string _saveFile = Application.persistentDataPath + "/" + _fileName;
            File.WriteAllText(_saveFile, jsonString);
        }

        public static void ReadData(ProfileScriptableObject _profileSO, string _fileName)
        {
            string _saveFile = Application.persistentDataPath + "/" + _fileName;
            if (File.Exists(_saveFile))
            {
                string fileContents = File.ReadAllText(_saveFile);
            }
            else
            {
                _profileSO.PlayerName = null;
            }
        }
    }
}