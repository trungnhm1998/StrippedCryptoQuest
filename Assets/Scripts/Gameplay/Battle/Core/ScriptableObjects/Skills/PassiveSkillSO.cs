using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "PassiveSkill", menuName = "Gameplay/Battle/Abilities/Passive Ability")]
    public class PassiveSkillSO : CQSkillSO
    {
        protected override AbstractAbility CreateAbility()
        {
            var skill = new PassiveSkill();
            return skill;
        }
    }

    public class PassiveSkill : CQSkill
    {
        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            base.OnAbilityGranted(skillSpec);
            Owner.TryActiveAbility(skillSpec);
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}