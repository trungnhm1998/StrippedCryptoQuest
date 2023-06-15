using UnityEngine;
using UnityEngine.Serialization;

namespace Core.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [FormerlySerializedAs("saveManagerSO")] [SerializeField] private SaveManagerSO _saveManagerSO;
        public SaveManagerSO SaveManagerSO => _saveManagerSO;

        [FormerlySerializedAs("saveData")] public SaveData _saveData = new SaveData();

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