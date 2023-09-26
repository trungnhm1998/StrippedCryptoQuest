using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Authoring
{
    public abstract class AbstractObjective : SerializableScriptableObject
    {
        [field: SerializeField] public string Id { get; set; }
        [field: SerializeField] public bool IsCompleted { get; protected set; }
        public UnityAction OnCompleteObjective { get; set; }
        public UnityAction OnObjectiveProgressChange { get; set; }

        public virtual void OnComplete()
        {
            IsCompleted = true;
            OnCompleteObjective?.Invoke();
        }

        public virtual void OnProgressChange() { }
    }
}