using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class CastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastableAbility _selectedSkill;
        private readonly Components.Character _target;

        public CastSkillCommand(Components.Character owner, CastableAbility selectedSkill, Components.Character target)
        {
            _target = target;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public void Execute()
        {
            Debug.Log($"{_owner.name} casting {_selectedSkill.name} on {_target.name}");
            var spec = _owner.AbilitySystem.GiveAbility<CastableAbilitySpec>(_selectedSkill);
            spec.Execute(_target.AbilitySystem);
        }
    }
}