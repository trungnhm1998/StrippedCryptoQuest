using System.Collections;
using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class CastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastableAbility _selectedSkill;

        public CastSkillCommand(Components.Character owner, CastableAbility selectedSkill, Components.Character target)
        {
            owner.Targeting.Target = target;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public IEnumerator Execute()
        {
            Debug.Log($"{_owner.name} casting {_selectedSkill.name} on {_owner.Targeting.Target.name}");
            var spec = _owner.AbilitySystem.GiveAbility<CastableAbilitySpec>(_selectedSkill);
            spec.Execute(_owner.Targeting.Target.AbilitySystem);
            yield break;
        }
    }
}