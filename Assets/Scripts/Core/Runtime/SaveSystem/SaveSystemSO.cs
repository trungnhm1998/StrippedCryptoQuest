using UnityEngine;

namespace Core.Runtime.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;
        public SaveManagerSO SaveManagerSO => _saveManagerSO;

        public SaveData _saveData = new SaveData();
        public string PlayerName
        {
            get => _saveData.playerName;
            set => _saveData.playerName = value;
        }

        public bool SaveGame()
        {
            return _saveManagerSO.Save(_saveData);
        }

        public bool LoadSaveGame()
        {
            return _saveManagerSO.Load(out _saveData);
        }
    }
}