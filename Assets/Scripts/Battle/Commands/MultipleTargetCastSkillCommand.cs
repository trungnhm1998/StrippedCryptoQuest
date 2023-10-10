using System.Collections;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class MultipleTargetCastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastSkillAbility _selectedSkill;
        private Components.Character[] _targets;

        public MultipleTargetCastSkillCommand(Components.Character owner,
            CastSkillAbility selectedSkill, params Components.Character[] targets)
        {
            _targets = targets;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public IEnumerator Execute()
        {
            BattleEventBus.RaiseEvent(new CastSkillEvent()
            {
                Character = _owner,
                Skill = _selectedSkill
            });
            var spec = _owner.AbilitySystem.GiveAbility<CastSkillAbilitySpec>(_selectedSkill);
            var targetSystems = _targets.Where(t => t.IsValidAndAlive())
                .Select(t => t.AbilitySystem).ToArray();
            Debug.Log($"{_owner.DisplayName} casting multiple target skill {_selectedSkill.name}");
            spec.Execute(targetSystems);
            yield break;
        }
    }
}