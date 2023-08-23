using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class BattlePrototype : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private Character _enemy;

        [SerializeField] private AbilityScriptableObject _abilityToUseOnEnemy;

        private SimpleGameplayAbilitySpec _gameplayAbilitySpec;

        public void GiveAbilityToCharacter()
        {
            _gameplayAbilitySpec = _character.GameplayAbilitySystem.GiveAbility(_abilityToUseOnEnemy) as SimpleGameplayAbilitySpec;
        }

        public void UseAbilityOnEnemy()
        {
            _gameplayAbilitySpec.Active(_enemy);
        }
    }
}