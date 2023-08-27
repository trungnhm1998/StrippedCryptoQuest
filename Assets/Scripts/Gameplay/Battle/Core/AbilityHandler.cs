using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core
{
    public class AbilityHandler : MonoBehaviour
    {
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

        public void ActivateSkill(AbilityScriptableObject abilityScriptableObject)
        {
            var ability = abilityScriptableObject.GetAbilitySpec(_abilitySystemBehaviour);
            ability.ActivateAbility();
        }
    }
}