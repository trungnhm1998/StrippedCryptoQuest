using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill/Target Enemy",
        fileName = "ConditionSkill")]
    public class ApplyEffectToTargetAbility : PostNormalAttackPassiveBase
    {
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }
        protected override GameplayAbilitySpec CreateAbility()
            => new ApplyEffectToTargetSpec(this);
#if UNITY_EDITOR
        public override PostNormalAttackPassiveBase CreateInstance()
        {
            return (ApplyEffectToTargetAbility)Activator.CreateInstance(this.GetType());
        }
#endif
    }

    public class ApplyEffectToTargetSpec : PostNormalAttackPassiveSpecBase
    {
        private ActiveGameplayEffect _activeGameplayEffect;
        private ApplyEffectToTargetAbility _ability;

        public ApplyEffectToTargetSpec(ApplyEffectToTargetAbility ability)
        {
            _ability = ability;
        }

        protected override void OnAttacked(DamageContext postAttackContext)
        {
            var target = postAttackContext.Target;
            if (!IsTargetValid(target)) return;
            if (FailedToActive(_ability)) return;
            RemovePreviousEffectIfExisted(ref _activeGameplayEffect);
            var postNormalAttackContext = new PostDamageContext(SkillContext)
            {
                DamageContext = postAttackContext
            };
            var gameplayEffectContextHandle = new GameplayEffectContextHandle(postNormalAttackContext);
            var effectSpec = _ability.Effect.CreateEffectSpec(Owner, gameplayEffectContextHandle);
            _activeGameplayEffect = target.AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }
    }
}