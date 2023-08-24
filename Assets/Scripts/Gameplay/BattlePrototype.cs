using CryptoQuest.Gameplay.Skill;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class BattlePrototype : MonoBehaviour
    {
        [SerializeField] private AbilityData _abilityToUseOnEnemy;

        private SimpleGameplayAbilitySpec _gameplayAbilitySpec;

        public void GiveAbilityToTarget(CharacterBehaviour character)
        {
            SimpleAbilitySO abilitySo = _abilityToUseOnEnemy.CreateAbilityInstance();
            _gameplayAbilitySpec = character.GameplayAbilitySystem.GiveAbility(abilitySo) as SimpleGameplayAbilitySpec;
        }

        public void UseAbilityOnTarget(CharacterBehaviour character)
        {
            _gameplayAbilitySpec.Active(character);
        }
    }
}