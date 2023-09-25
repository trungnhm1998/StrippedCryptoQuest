using System;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    [CreateAssetMenu(fileName = "Task", menuName = "Quest System/Task")]
    public class Task : AbstractObjective
    {
        public virtual bool EqualTo(Task other)
        {
            return this == other;
        }

        public override void OnComplete()
        {
            Debug.Log($"Task: <color=green>{name}</color> was completed.");
            base.OnComplete();
        }

        public override void OnProgressChange()
        {
            //TODO: implement task count. Ex: Kill 10 enemies
            OnObjectiveProgressChange?.Invoke();
        }
    }
}