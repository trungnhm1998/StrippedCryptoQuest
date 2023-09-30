using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class CastSkillCommand : ICommand
    {
        private HeroBehaviour _hero;
        private CastableAbility _selectedSkill;
        private EnemyBehaviour _enemy;

        public CastSkillCommand(HeroBehaviour hero, CastableAbility selectedSkill, EnemyBehaviour enemy)
        {
            _enemy = enemy;
            _selectedSkill = selectedSkill;
            _hero = hero;
        }

        public void Execute()
        {
            Debug.Log($"{_hero.name} casting {_selectedSkill.name} on {_enemy.name}");
            _hero.TryGetComponent(out AbilitySystemBehaviour abilitySystem);
            var spec = abilitySystem.GiveAbility<CastableAbilitySpec>(_selectedSkill);
            spec.Execute(_enemy.GetComponent<AbilitySystemBehaviour>());
        }
    }
}