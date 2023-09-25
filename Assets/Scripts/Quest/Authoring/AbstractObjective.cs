using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Authoring
{
    public abstract class AbstractObjective : SerializableScriptableObject, IObjective
    {
        [field: SerializeField] public string Id { get; set; }
        [field: SerializeField] public bool IsCompleted { get; protected set; }
        public UnityAction OnCompleteObjective { get; set; }
        public UnityAction OnObjectiveProgressChange { get; set; }

#if UNITY_EDITOR

        /// <summary>
        /// This method is used by the editor to set the completed state of the objective
        /// </summary>
        /// <param name="value"></param>
        public void Editor_SetCompleted(bool value)
        {
            IsCompleted = value;
        }

#endif
        public virtual void OnComplete()
        {
            IsCompleted = true;
            OnCompleteObjective?.Invoke();
        }

        public virtual void OnProgressChange() { }

        public virtual void SubscribeObjective() { }

        public virtual void UnsubscribeObjective() { }
    }
}