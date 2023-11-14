using IndiGames.Core.SaveSystem.ScriptableObjects;

namespace CryptoQuest.SaveSystem
{
    public abstract class SaveManagerSO : SerializableScriptableObject
    {
        public abstract bool Save(string saveData);

        public abstract bool Load(out string saveData);
    }
}
