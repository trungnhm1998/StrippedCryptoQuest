using IndiGames.Core.SaveSystem.ScriptableObjects;
using System.Threading.Tasks;

namespace CryptoQuest.System.SaveSystem
{
    public abstract class SaveManagerSO : SerializableScriptableObject
    {
        public abstract bool Save(string saveData);

        public abstract string Load();

        public abstract Task<bool> SaveAsync(string saveData);

        public abstract Task<string> LoadAsync();
    }
}
