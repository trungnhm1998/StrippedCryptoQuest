using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;
        public string saveFileName = "save.json";

        public SaveData saveData = new SaveData();

        public bool SaveData()
        {
            return false;
        }

        public bool LoadData()
        {
            return false;
        }
    }
}