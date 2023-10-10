using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.SaveSystem
{
    [Serializable] public class SerializabeEvent : UnityEvent { }

    public abstract class SaveManagerSO : ScriptableObject
    {
        public abstract Task<bool> SaveAsync(SaveData saveData);

        public abstract Task<SaveData> LoadAsync();
    }
}
