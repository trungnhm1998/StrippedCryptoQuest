using System.Collections;
using System.Linq;
using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class MultipleTargetCastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastableAbility _selectedSkill;
        private Components.Character[] _targets;

        public MultipleTargetCastSkillCommand(Components.Character owner,
            CastableAbility selectedSkill, params Components.Character[] targets)
        {
            _targets = targets;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public IEnumerator Execute()
        {
            var spec = _owner.AbilitySystem.GiveAbility<CastableAbilitySpec>(_selectedSkill);
            var targetSystems = _targets.Where(t => t.IsValid())
                .Select(t => t.AbilitySystem).ToArray();
            Debug.Log($"{_owner.DisplayName} casting multiple target skill {_selectedSkill.name}");
            spec.Execute(targetSystems);
            yield break;
        }
    }
}