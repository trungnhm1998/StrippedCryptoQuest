using CryptoQuest.Quest.Actor;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/ActiveActorEvent", fileName = "ActiveActorEventChannelSO", order = 0)]
    public class ActiveActorEventChannelSO : ScriptableObject
    {
        public UnityAction<ActorSO, bool> EventRaised;

        public void RaiseEvent(ActorSO actor, bool active)
        {
            OnRaiseEvent(actor, active);
        }

        protected virtual void OnRaiseEvent(ActorSO actor, bool active)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(actor, active);
        }
    }
}