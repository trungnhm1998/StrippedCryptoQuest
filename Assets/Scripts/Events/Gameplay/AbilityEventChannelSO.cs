using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
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
            this.CallEventSafely(EventRaised, ability);
        }
    }
}