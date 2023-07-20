using System.Collections;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "InstantAbilitySO", menuName = "Gameplay/Battle/Abilities/Instant Ability")]
    public class InstantAbilitySO : CQSkillSO
    {
        public SkillParameters SkillParams;
        protected override AbstractAbility CreateAbility() => new InstantAbility(SkillParams);
    }


    public class InstantAbility : CQSkill
    {
        protected SkillParameters _skillParameters;

        protected new InstantAbilitySO AbilitySO => (InstantAbilitySO)_abilitySO;

        public InstantAbility(SkillParameters skillParameters)
        {
            _skillParameters = skillParameters;
        }

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