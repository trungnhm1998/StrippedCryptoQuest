using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class BattlePrototype : MonoBehaviour
    {
        [SerializeField] private SimpleAbilitySO _ability;

        private SimpleGameplayAbilitySpec _gameplayAbilitySpec;

        public void GiveAbilityToTarget(CharacterBehaviour character)
        {
            _gameplayAbilitySpec = character.GameplayAbilitySystem.GiveAbility(_ability) as SimpleGameplayAbilitySpec;
        }

        public void UseAbilityOnTarget(CharacterBehaviour character)
        {
            _gameplayAbilitySpec.Active(character);
        }
    }
}