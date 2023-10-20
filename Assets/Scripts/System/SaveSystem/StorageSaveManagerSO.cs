using UnityEngine;
using System;
using System.Threading.Tasks;

#if UNITY_EDITOR || !UNITY_WEBGL
using System.IO;
using System.Threading;
#endif

namespace CryptoQuest.System.SaveSystem
{
    public class StorageSaveManagerSO : SaveManagerSO
    {
        [Header("Save Config")]
        [SerializeField] protected string fileName;
        [SerializeField] protected bool useEncryption;
        [SerializeField] protected string encryptionCode;

        public override bool Save(string saveData)
        {
            // serialize the save data into json
            var jsonData = saveData;
            Debug.Log("Save() jsonData: " + jsonData);

            // optionally encrypt the data
            if (useEncryption)
            {
                jsonData = EncryptDecrypt(jsonData);
            }
            Debug.Log("Save() jsonData length: " + jsonData.Length);

#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(fileName, jsonData);
            return true;
#else
            // use Path.Combine to account for different OS's having different path separators
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log("Save() filePath: " + filePath);

            const int RETRY_MAX_COUNT = 3;
            try
            {
                var saved = false;
                var retryCount = 0;
                while (!saved && retryCount < RETRY_MAX_COUNT)
                {
                    try
                    {
                        // write the serialized data to the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            using (var writer = new StreamWriter(stream))
                            {
                                writer.Write(jsonData);
                            }
                        }
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error occured when trying to save, retry = " + (retryCount + 1) + "\n" + e);
                        // Retry after 1 seconds
                        Thread.Sleep(1000);
                    }
                    retryCount++;
                }
                Debug.Log("Save() return = " + saved);
                return saved;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + filePath + "\n" + e);
            }
            Debug.Log("Save() return = " + false);          
            return false;
#endif
        }

        public override string Load()
        {
            // load the serialized data from the file
            string jsonData = null;

#if UNITY_WEBGL && !UNITY_EDITOR
            jsonData = PlayerPrefs.GetString(fileName, null);
#else
            // use Path.Combine to account for different OS's having different path separators
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log("LoadSave() filePath: " + filePath);
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open))
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
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + filePath + "\n" + e);
            }
#endif
            if (string.IsNullOrEmpty(jsonData))
            {
                return null;
            }

            Debug.Log("LoadSave() jsonData: " + jsonData);
            Debug.Log("LoadSave() jsonData length: " + jsonData.Length);
            return jsonData;
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

        public override async Task<bool> SaveAsync(string saveData)
        {
            return await Task.Run(() => Save(saveData));
        }

        public override async Task<string> LoadAsync()
        {
            return await Task.Run(() => Load());
        }
    }
}
