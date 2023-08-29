using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay.Skill;
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

        [SerializeField] private AbilityData _abilityData;

        public void GiveAbilityByData(CharacterBehaviourBase characterBehaviour)
        {
            var abilityController = characterBehaviour.GetComponent<IAbilityController>();
            var ability = abilityController.InitAbility(_abilityData);
            _gameplayAbilitySpec =
                (SimpleGameplayAbilitySpec)characterBehaviour.GameplayAbilitySystem.GiveAbility(ability);
        }
    }
}