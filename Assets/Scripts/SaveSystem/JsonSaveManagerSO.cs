using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class JsonSaveManagerSO : SaveManagerSO
    {
        public string saveFileName = "save.json";

        public override bool Save(SaveData saveData)
        {
            var saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

            try
            {
                File.WriteAllText(saveFilePath, JsonConvert.SerializeObject(saveData));
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public override bool Load(out SaveData saveData)
        {
            var saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

            CreateNewSaveFileIfNotExists(saveFilePath);

            try
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(saveFilePath));
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                saveData = new SaveData();
                return false;
            }
        }

        private static void CreateNewSaveFileIfNotExists(string saveFilePath)
        {
            if (!File.Exists(saveFilePath))
            {
                File.WriteAllText(saveFilePath, "");
            }
        }
    }
}