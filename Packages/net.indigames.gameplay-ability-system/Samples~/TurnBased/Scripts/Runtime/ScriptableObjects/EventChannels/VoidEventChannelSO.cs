using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    [CreateAssetMenu(menuName = "Indigames Ability System/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction EventRaised;

        public void RaiseEvent()
        {
            OnRaiseEvent();
        }

        private void OnRaiseEvent()
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke();
        }
    }
}