using UnityEngine;
using System;
using System.IO;
using IndiGames.Core.SaveSystem;
using System.Threading.Tasks;

namespace CryptoQuest.System.Save
{
    public class StorageSaveManagerSO : SaveManagerSO
    {
        [Header("Save Config")]
        [SerializeField] protected string fileName;
        [SerializeField] protected bool useEncryption;
        [SerializeField] protected string encryptionCode;

        public async override Task<bool> SaveAsync(SaveData saveData)
        {
            // use Path.Combine to account for different OS's having different path separators
            var tmpPath = Path.Combine(Application.persistentDataPath, "Prefs", "tmp_" + fileName);
            var filePath = Path.Combine(Application.persistentDataPath, "Prefs", fileName);
            try
            {
                // create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(tmpPath));

                // serialize the save data into json
                var jsonData = saveData.ToJson();

                // optionally encrypt the data
                if (useEncryption)
                {
                    jsonData = EncryptDecrypt(jsonData);
                }

                try
                {
                    if (File.Exists(tmpPath))
                    {
                        File.Delete(tmpPath);
                    }
                } catch { }

                // write the serialized data to the file
                using (var stream = new FileStream(tmpPath, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonData);
                    }
                }

                await Task.Delay(1);

                var saved = false;
                while (!saved)
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        File.Move(tmpPath, filePath);
                        saved = true;
                    }
                    catch { }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + filePath + "\n" + e);
            }
            return false;
        }

        public async override Task<SaveData> LoadAsync()
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

                    await Task.Delay(1);

                    // optionally decrypt the data
                    if (useEncryption)
                    {
                        jsonData = EncryptDecrypt(jsonData);
                    }

                    // load saveData from json
                    var saveData = new SaveData();
                    saveData.LoadFromJson(jsonData);
                    return saveData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
            return null;
        }

        //Simple XOR encryption/decryption
        protected string EncryptDecrypt(string data)
        {
            // if encryption code is not set, return non-encrypted data
            if (!useEncryption || encryptionCode.Length == 0)
            {
                return data;
            }

            // simple XOR encryption
            var modifiedData = "";
            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
            }
            return modifiedData;
        }
    }
}
