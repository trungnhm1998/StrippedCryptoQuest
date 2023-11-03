using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill/Target Self", fileName = "ConditionSkill")]
    public class ApplyEffectToSelfAbility : PostNormalAttackPassiveBase
    {
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }
        protected override GameplayAbilitySpec CreateAbility()
            => new ApplyEffectToSelfSpec(this);
    }

    public class ApplyEffectToSelfSpec : PostNormalAttackPassiveSpecBase
    {
        private ActiveGameplayEffect _activeGameplayEffect;
        private ApplyEffectToSelfAbility _ability;

        public ApplyEffectToSelfSpec(ApplyEffectToSelfAbility ability)
        {
            _ability = ability;
        }

        protected override void OnAttacked(DamageContext postAttackContext)
        {
            if (!IsTargetValid(Character)) return;
            if (FailedToActive(_ability)) return;
            RemovePreviousEffectIfExisted(ref _activeGameplayEffect);
            var postNormalAttackContext = new PostDamageContext(SkillContext)
            {
                DamageContext = postAttackContext
            };
            var gameplayEffectContextHandle = new GameplayEffectContextHandle(postNormalAttackContext);
            var effectSpec = _ability.Effect.CreateEffectSpec(Owner, gameplayEffectContextHandle);
            _activeGameplayEffect = Character.AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }
    }
}