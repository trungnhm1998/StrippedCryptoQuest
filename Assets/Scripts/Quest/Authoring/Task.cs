using System;
using UnityEngine;

namespace CryptoQuest.Quests
{
    public abstract class Task : ScriptableObject, IComparable<Task>
    {
        [field: SerializeField] public string Id { get; set; }

        public virtual int CompareTo(Task other)
        {
            return this == other ? 0 : -1;
        }
    }
}