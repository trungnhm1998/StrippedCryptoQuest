using UnityEngine;

namespace Core.SaveSystem
{
    public abstract class SaveManagerSO : ScriptableObject
    {
        public abstract bool Save(SaveData saveData);

        public abstract bool Load(out SaveData saveData);
    }
}