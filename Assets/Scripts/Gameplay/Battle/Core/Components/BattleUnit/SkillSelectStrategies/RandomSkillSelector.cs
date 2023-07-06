using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
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