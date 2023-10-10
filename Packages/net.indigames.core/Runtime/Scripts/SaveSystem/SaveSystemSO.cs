using System.Threading.Tasks;
using UnityEngine;

namespace IndiGames.Core.SaveSystem
{
    public class SaveSystemSO : ScriptableObject
    {
        [SerializeField] private SaveManagerSO _saveManagerSO;

        [SerializeField] private SaveData _saveData = new SaveData();

        public SaveData SaveData
        {
            get => _saveData;
            set => _saveData = value;
        }

        public string PlayerName
        {
            get => _saveData.playerName;
            set => _saveData.playerName = value;
        }

        public async Task<bool> SaveGame()
        {
            return await _saveManagerSO.SaveAsync(_saveData);
        }

        public async Task<bool> LoadSaveGame()
        {
            _saveData = await _saveManagerSO.LoadAsync();
            return _saveData != null;
        }
    }
}