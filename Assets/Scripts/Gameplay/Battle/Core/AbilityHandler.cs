using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest
{
    public class AbilityHandler : MonoBehaviour
    {
        [SerializeField] private AbilityEventChannelSO _onRequestActivateAbility;
        [SerializeField] private AbilitySystemBehaviour _abilitySystemBehaviour;
#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateComponents();
        }
#endif

        private void ValidateComponents()
        {
            if (_abilitySystemBehaviour == null)
            {
                _abilitySystemBehaviour = GetComponent<AbilitySystemBehaviour>();
            }
        }

        private void OnEnable()
        {
            _onRequestActivateAbility.EventRaised += ActivateSkill;
        }

        private void OnDisable()
        {
            _onRequestActivateAbility.EventRaised -= ActivateSkill;
        }

        public void ActivateSkill(AbilityScriptableObject abilityScriptableObject)
        {
            var ability = _abilitySystemBehaviour.GiveAbility(abilityScriptableObject);
            _abilitySystemBehaviour.TryActiveAbility(ability);
        }
    }
}