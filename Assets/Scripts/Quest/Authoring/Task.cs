using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [Serializable]
    [CreateAssetMenu(fileName = "Task", menuName = "Quest System/Task")]
    public class Task : AbstractObjective
    {
        [field: SerializeField] public string Id { get; set; }

        public virtual int CompareTo(Task other)
        {
            return this == other ? 0 : -1;
        }

        public override void OnComplete()
        {
            IsCompleted = true;
            OnCompleteObjective?.Invoke();
        }

        public override void OnProgressChange()
        {
            //TODO: implement task count. Ex: Kill 10 enemies
            OnObjectiveProgressChange?.Invoke();
        }

        public override void SubscribeObjective() { }

        public override void UnsubscribeObjective() { }
    }
}