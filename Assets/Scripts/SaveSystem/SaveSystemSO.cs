using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;
        public SaveManagerSO SaveManagerSO => _saveManagerSO;

        public SaveData saveData = new SaveData();

        public bool SaveGame()
        {
            return _saveManagerSO.Save(saveData);
        }

        public bool LoadSaveGame()
        {
            return _saveManagerSO.Load(out saveData);
        }
    }
}