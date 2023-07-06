using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "PassiveSkill", menuName = "Gameplay/Battle/Abilities/Passive Ability")]
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