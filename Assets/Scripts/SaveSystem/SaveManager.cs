using Newtonsoft;
using System;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace CryptoQuest.SaveSystem
{
    [Serializable]
    public class SaveManager
    {
        public struct SaveData
        {
            public string playerName;
        }
        
        public SaveData saveData;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(saveData);
        }

        public void LoadFromJson(string json)
        {
            saveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}