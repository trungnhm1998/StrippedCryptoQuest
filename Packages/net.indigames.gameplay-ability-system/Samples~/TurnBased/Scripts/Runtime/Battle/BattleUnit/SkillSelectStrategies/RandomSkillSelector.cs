using Indigames.AbilitySystem;
using UnityEngine;

namespace Indigames.AbilitySystem.Sample
{
    public class RandomSkillSelector : ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit)
        {
            var grantedSkills = battleUnit.Owner.GrantedAbilities.Abilities;
            var ramdomSkill = grantedSkills[Random.Range(0, grantedSkills.Count - 1)];
            return ramdomSkill;
        }
    }
}