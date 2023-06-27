using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "PassiveSkill", menuName = "Indigames Ability System/Skills/Passive Skill")]
    public class PassiveSkillSO : EffectSkillSO
    {
        protected override AbstractSkill CreateSkill()
        {
            var skill = new PassiveSkill();
            return skill;
        }
    }

    public class PassiveSkill : EffectSkill
    {
        public override void OnSkillGranted(AbstractSkill skillSpec)
        {
            Owner.SkillSystem.TryActiveSkill(skillSpec);
        }

        public override bool CanActiveSkill()
        {
            return !IsActive && base.CanActiveSkill();
        }
    }
}