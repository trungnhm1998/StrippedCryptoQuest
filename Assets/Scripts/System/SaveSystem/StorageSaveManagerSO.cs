#if !UNITY_WEBGL || UNITY_EDITOR
#define USE_FILE_SYSTEM
#endif

using UnityEngine;
#if USE_FILE_SYSTEM
#endif

namespace CryptoQuest.SaveSystem
{
    public class StorageSaveManagerSO : SaveManagerSO
    {
        [Header("Save Config")]
        [SerializeField] private string _fileName = "game.sav";

        [SerializeField] private string _backupFileName = "game.sav.bak";
        [SerializeField] private bool _useEncryption;
        [SerializeField] private string _encryptionCode = "CryptoQuestIndiGames";

        public override bool Save(string saveData)
        {
            // serialize the save data into json
            var jsonData = saveData;

            // optionally encrypt the data
            if (_useEncryption)
            {
                jsonData = EncryptDecrypt(jsonData);
            }

#if USE_FILE_SYSTEM
            // TODO: PREVENT WRITE FREQUENTLY OR USE BUFFER/THREAD
            FileManager.MoveFile(_fileName, _backupFileName);
            return FileManager.WriteToFile(_fileName, jsonData);
#else
            PlayerPrefs.SetString(_fileName, jsonData);
            return true;
#endif
        }

        public override bool Load(out string json)
        {
#if USE_FILE_SYSTEM
            if (FileManager.LoadFromFile(_fileName, out json))
                return true;
#else
            json = PlayerPrefs.GetString(_fileName, null);
#endif
            return string.IsNullOrEmpty(json);
        }

        //Simple XOR encryption/decryption
        protected string EncryptDecrypt(string data)
        {
            // if encryption code is not set, return non-encrypted data
            if (!_useEncryption || _encryptionCode.Length == 0)
            {
                return data;
            }

            // simple XOR encryption
            var modifiedData = "";
            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _encryptionCode[i % _encryptionCode.Length]);
            }

            return modifiedData;
        }
    }
}