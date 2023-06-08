using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CryptoQuestClient
{
    public class SaveManagerSO : ScriptableObject
    {
        public SaveManagerSO(ProfileScriptableObject profile, string fileName)
        {
            _profileSO = profile;
            _fileName = fileName;
        }
        
        [Header("References")]
        public ProfileScriptableObject _profileSO;
        public string _fileName;

        public void SaveData()
        {
            ProfileManager.WriteData(_profileSO, _fileName);
        }

        public void LoadData()
        {
            ProfileManager.ReadData(_profileSO, _fileName);
        }
    }
}