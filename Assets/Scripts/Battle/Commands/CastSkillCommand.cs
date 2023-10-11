using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class CastSkillCommand : ICommand
    {
        private readonly Components.Character _owner;
        private readonly CastSkillAbility _selectedSkill;

        public CastSkillCommand(Components.Character owner, CastSkillAbility selectedSkill, Components.Character target)
        {
            owner.Targeting.Target = target;
            _selectedSkill = selectedSkill;
            _owner = owner;
        }

        public void Execute()
        {
            BattleEventBus.RaiseEvent(new CastSkillEvent()
            {
                Character = _owner,
                Skill = _selectedSkill
            });
            Debug.Log($"{_owner.DisplayName} casting {_selectedSkill.name} on {_owner.Targeting.Target.DisplayName}");
            var spec = _owner.AbilitySystem.GiveAbility<CastSkillAbilitySpec>(_selectedSkill);
            spec.Execute(_owner.Targeting.Target.AbilitySystem);
        }
    }
}