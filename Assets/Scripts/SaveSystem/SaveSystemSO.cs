using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;

        public SaveData saveData = new SaveData();

        public bool SaveData()
        {
            return _saveManagerSO.Save(saveData);
        }

        public bool LoadData()
        {
            return _saveManagerSO.Load(out saveData);
        }
    }
}