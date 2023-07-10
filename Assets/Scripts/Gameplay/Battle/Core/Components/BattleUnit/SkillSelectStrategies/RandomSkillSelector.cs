using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle
{
    public class RandomSkillSelector : ISkillSelector
    {
        public AbstractAbility GetSkill(BattleUnitBase battleUnit)
        {
            List<AbstractAbility> grantedSkills = battleUnit.Owner.GrantedAbilities.Abilities;
            AbstractAbility ramdomSkill = grantedSkills[Random.Range(0, grantedSkills.Count - 1)];
            return ramdomSkill;
        }
    }
}