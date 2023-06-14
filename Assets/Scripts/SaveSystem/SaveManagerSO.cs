using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public abstract class SaveManagerSO : ScriptableObject
    {
        public abstract bool Save(SaveData saveData);

        public abstract bool Load(out SaveData saveData);
    }
}