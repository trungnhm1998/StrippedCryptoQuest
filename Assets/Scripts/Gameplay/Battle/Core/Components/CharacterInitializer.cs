using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    [RequireComponent(typeof(AbilitySystemBehaviour))]
    public class CharacterInitializer : MonoBehaviour
    {
        [SerializeField] private AbilitySystemBehaviour _abilitySystem;
        [SerializeField] private CharacterDataSO _characterData;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_abilitySystem != null) return;
            _abilitySystem = GetComponent<AbilitySystemBehaviour>();
        }
#endif

        private void Start()
        {
            GrantDefaulSkills();
        }
        
        private void GrantDefaulSkills()
        {
            foreach (var skill in _characterData.GrantedSkills)
            {
                _abilitySystem.GiveAbility(skill);
            }
        }
    }
}