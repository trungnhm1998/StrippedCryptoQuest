using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.SkillSelectStrategies
{
    public class RandomSkillSelector : ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit)
        {
            List<AbstractAbility> grantedSkills = battleUnit.Owner.GrantedAbilities.Abilities;
            if (grantedSkills.Count <= 0) return null;
            AbstractAbility ramdomSkill = grantedSkills[Random.Range(0, grantedSkills.Count - 1)];
            return ramdomSkill;
        }
    }
}