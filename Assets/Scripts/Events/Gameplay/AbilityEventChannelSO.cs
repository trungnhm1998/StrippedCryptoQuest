using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(fileName = "AbilityEventChannelSO", menuName = "Core/Events/Ability Event Channel")]
    public class AbilityEventChannelSO : ScriptableObject
    {
        public UnityAction<AbilityScriptableObject> EventRaised;
#if UNITY_EDITOR
        public AbilityScriptableObject AbilitySO;
#endif
        public void RaiseEvent(AbilityScriptableObject ability)
        {
            OnRaiseEvent(ability);
#if UNITY_EDITOR
            AbilitySO = ability;
#endif
        }

        private void OnRaiseEvent(AbilityScriptableObject ability)
        {
            this.CallEventSafely(EventRaised, ability);
        }
    }
}