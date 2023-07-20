using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    public class AbilityEventChannelSO : ScriptableObject
    {
        public UnityAction<AbilityScriptableObject> EventRaised;

        public void RaiseEvent(AbilityScriptableObject ability)
        {
            OnRaiseEvent(ability);
        }

        private void OnRaiseEvent(AbilityScriptableObject ability)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(ability);
        }
    }
}