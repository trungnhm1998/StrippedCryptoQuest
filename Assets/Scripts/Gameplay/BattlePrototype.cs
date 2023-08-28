using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class BattlePrototype : MonoBehaviour
    {
        [SerializeField] private SimpleAbilitySO _ability;

        private SimpleGameplayAbilitySpec _gameplayAbilitySpec;

        public void GiveAbilityToTarget(CharacterBehaviourBase character)
        {
            _gameplayAbilitySpec = character.GameplayAbilitySystem.GiveAbility(_ability) as SimpleGameplayAbilitySpec;
        }

        public void UseAbilityOnTarget(CharacterBehaviourBase character)
        {
            _gameplayAbilitySpec.Active(character);
        }
    }
}