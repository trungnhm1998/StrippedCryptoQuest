using UnityEngine;
using System;
using System.IO;
using IndiGames.Core.SaveSystem;


namespace CryptoQuest.System.Save
{
    public class StorageSaveManagerSO : SaveManagerSO
    {
        public override bool Save(SaveData saveData)
        {
            // use Path.Combine to account for different OS's having different path separators
            var fullPath = Path.Combine(Application.persistentDataPath, "Prefs", fileName);
            try
            {
                // create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // serialize the save data into json
                var jsonData = saveData.ToJson();

                // optionally encrypt the data
                if (useEncryption)
                {
                    jsonData = EncryptDecrypt(jsonData);
                }

                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
            
            return true;
        }

        public override bool Load(out SaveData saveData)
        {
            // use Path.Combine to account for different OS's having different path separators
            var fullPath = Path.Combine(Application.persistentDataPath, "Prefs", fileName);
            try
            {
                if (File.Exists(fullPath))
                {                    
                    // load the serialized data from the file
                    string jsonData = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            jsonData = reader.ReadToEnd();
                        }
                    }

                    // optionally decrypt the data
                    if (useEncryption)
                    {
                        jsonData = EncryptDecrypt(jsonData);
                    }

                    // load saveData from json
                    saveData = new SaveData();
                    saveData.LoadFromJson(jsonData);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
            saveData = null;
            return false;
        }
    }
}
