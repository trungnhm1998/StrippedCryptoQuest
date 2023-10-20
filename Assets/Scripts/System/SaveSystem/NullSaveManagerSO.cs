using System.Threading.Tasks;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    public class NullSaveManagerSO : SaveManagerSO
    {
        private string saveData;

        public override bool Save(string saveData)
        {
            this.saveData = saveData;
            return true;
        }

        public override string Load()
        {
            return saveData;
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