using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    /// <summary>
    /// Skill will end right after activate like normal attack
    /// </summary>
    [CreateAssetMenu(fileName = "InstantSkill", menuName = "Gameplay/Battle/Abilities/Instant Skill")]
    public class InstantSkillSO : CQSkillSO
    {
        protected override AbstractAbility CreateAbility()
        {
            var skill = new InstantSkill();
            return skill;
        }
    }

    public class InstantSkill : CQSkill
    {
        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
            EndAbility();
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}