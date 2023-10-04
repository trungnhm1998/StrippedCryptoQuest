using UnityEngine;

namespace IndiGames.Core.SaveSystem
{
    public abstract class SaveManagerSO : ScriptableObject
    {
        [Header("Save Config")]
        [SerializeField] protected string fileName;
        [SerializeField] protected bool useEncryption;
        [SerializeField] protected string encryptionCode; 
        
        public abstract bool Save(SaveData saveData);

        public abstract bool Load(out SaveData saveData);

        //Simple XOR encryption/decryption
        protected string EncryptDecrypt(string data)
        {
            // if encryption code is not set, return non-encrypted data
            if (!useEncryption || encryptionCode.Length == 0)
            {
                return data;
            }

            // simple XOR encryption
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
            }
            return modifiedData;
        }
    }
}