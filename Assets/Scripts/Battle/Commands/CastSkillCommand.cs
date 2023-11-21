using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class CastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastSkillAbility _selectedSkill;
        private Components.Character _targetingTarget;
        private CastSkillAbilitySpec _spec;

        public CastSkillCommand(Components.Character owner, CastSkillAbility selectedSkill, Components.Character target)
        {
            _targetingTarget = target;
            owner.Targeting.Target = _targetingTarget;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public void Execute()
        {
            Debug.Log($"{_owner.DisplayName} casting {_selectedSkill.name} on {_owner.Targeting.Target.DisplayName}");
            _spec = _owner.AbilitySystem.GiveAbility<CastSkillAbilitySpec>(_selectedSkill);
            _spec.Execute(GetCurrentTarget().AbilitySystem);
        }

        private Components.Character GetCurrentTarget()
        {
            if ((_spec.Def.TargetType.Target | SkillTargetType.Type.SameTeam) == SkillTargetType.Type.SameTeam)
                return _targetingTarget;
            return _owner.Targeting.Target;
        }
    }
}