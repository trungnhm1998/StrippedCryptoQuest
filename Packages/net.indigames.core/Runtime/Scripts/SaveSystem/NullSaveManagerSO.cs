using System.Threading.Tasks;

namespace IndiGames.Core.SaveSystem
{
    public class NullSaveManagerSO : SaveManagerSO
    {
        private SaveData saveData;

        public override async Task<bool> SaveAsync(SaveData saveData)
        {
            this.saveData = saveData;
            await Task.Delay(1);
            return true;
        }

        public override async Task<SaveData> LoadAsync()
        {
            await Task.Delay(1);
            return this.saveData;
        }
    }
}