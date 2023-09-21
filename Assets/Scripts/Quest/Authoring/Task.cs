using System;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    [CreateAssetMenu(fileName = "Task", menuName = "Quest System/Task")]
    public class Task : AbstractObjective
    {
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