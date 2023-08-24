using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class BattlePrototype : MonoBehaviour
    {
        [SerializeField] private AbilityScriptableObject _abilityToUseOnEnemy;

        private SimpleGameplayAbilitySpec _gameplayAbilitySpec;

        public void GiveAbilityToTarget(Character character)
        {
            _gameplayAbilitySpec = character.GameplayAbilitySystem.GiveAbility(_abilityToUseOnEnemy) as SimpleGameplayAbilitySpec;
        }

        public void UseAbilityOnTarget(Character character)
        {
            _gameplayAbilitySpec.Active(character);
        }
    }
}