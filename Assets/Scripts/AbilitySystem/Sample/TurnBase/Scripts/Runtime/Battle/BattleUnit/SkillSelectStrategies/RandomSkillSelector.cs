using Indigames.AbilitySystem;
using UnityEngine;

namespace Indigames.AbilitySystem.Sample
{
    public class RandomSkillActivator : ISkillActivator
    {
        public void ActivateSkill(BattleUnitBase battleUnit)
        {
            var grantedSkills = battleUnit.Owner.GrantedAbilities.Abilities;
            var ramdomSkill = grantedSkills[Random.Range(0, grantedSkills.Count - 1)];
            battleUnit.Owner.TryActiveAbility(ramdomSkill);
        }
    }
}