using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using UnityEngine;
using CryptoQuest.Battle.Components.HeroComponents;

namespace CryptoQuest.Battle.Commands
{
    public class EnemyGroupCastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastSkillAbility _selectedSkill;
        private HeroTargetEnemyGroupBehaviour _targetGroup;

        public EnemyGroupCastSkillCommand(Components.Character owner,
            CastSkillAbility selectedSkill, EnemyGroup enemyGroup)
        {
            _targetGroup = owner.GetComponent<HeroTargetEnemyGroupBehaviour>();
            _targetGroup.EnemyGroup = enemyGroup;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public void Execute()
        {
            var spec = _owner.AbilitySystem.GiveAbility<CastSkillAbilitySpec>(_selectedSkill);
            var targetSystems = _targetGroup.EnemyGroup.GetAliveEnemies()
                .Select(e => e.AbilitySystem).ToArray();
            Debug.Log($"{_owner.DisplayName} casting enemy group skill {_selectedSkill.name}");
            spec.Execute(targetSystems);
        }
    }
}