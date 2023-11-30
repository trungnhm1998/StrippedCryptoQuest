using IndiGames.Core.SaveSystem.ScriptableObjects;

namespace CryptoQuest.System.SaveSystem
{
    public abstract class SaveManagerSO : SerializableScriptableObject
    {
        public abstract bool Save(string saveData);

        public abstract bool Load(out string saveData);
    }
}
