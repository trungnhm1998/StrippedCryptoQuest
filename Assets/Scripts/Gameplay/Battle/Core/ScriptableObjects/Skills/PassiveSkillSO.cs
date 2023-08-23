using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "PassiveSkill", menuName = "Gameplay/Battle/Abilities/Passive Ability")]
    public class PassiveSkillSO : CQSkillSO
    {
        protected override GameplayAbilitySpec CreateAbility()
        {
            var skill = new PassiveSkill();
            return skill;
        }
    }

    public class PassiveSkill : CQSkill
    {
        public override void OnAbilityGranted(GameplayAbilitySpec skill)
        {
            base.OnAbilityGranted(skill);
            Owner.TryActiveAbility(skill);
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}