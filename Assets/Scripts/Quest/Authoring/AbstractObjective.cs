using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Authoring
{
    public abstract class AbstractObjective : ScriptableObject
    {
        [field: SerializeField] public string Id { get; set; }
        [field: SerializeField] public bool IsCompleted { get; protected set; }
        public UnityAction OnCompleteObjective { get; set; }
        public UnityAction OnObjectiveProgressChange { get; set; }


        public abstract void OnComplete();
        public abstract void SubscribeObjective();
        public abstract void UnsubscribeObjective();
        public abstract void OnProgressChange();
    }
}