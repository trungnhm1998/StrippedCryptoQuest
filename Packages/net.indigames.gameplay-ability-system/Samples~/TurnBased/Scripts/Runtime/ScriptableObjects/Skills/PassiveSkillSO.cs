using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "PassiveSkill", menuName = "Indigames Ability System/Abilities/Passive Ability")]
    public class PassiveSkillSO : EffectAbilitySO
    {
        protected override AbstractAbility CreateAbility()
        {
            var skill = new PassiveSkill();
            return skill;
        }
    }

    public class PassiveSkill : EffectAbility
    {
        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            Owner.TryActiveAbility(skillSpec);
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}