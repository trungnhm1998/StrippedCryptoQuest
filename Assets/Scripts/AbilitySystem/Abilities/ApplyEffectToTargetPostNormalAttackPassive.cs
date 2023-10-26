using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill", fileName = "ConditionSkill")]
    public class ApplyEffectToTargetPostNormalAttackPassive : PassiveAbility
    {
        [field: SerializeField, Range(0, 1f)] public float SuccessRate { get; private set; } = 1f;
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }

        protected override GameplayAbilitySpec CreateAbility()
            => new ApplyEffectPassivePostNormalAttackSpec();
    }

    public class ApplyEffectPassivePostNormalAttackSpec : PostNormalAttackPassiveSpecBase
    {
        private ActiveGameplayEffect _activeGameplayEffect;
        private new ApplyEffectToTargetPostNormalAttackPassive _abilitySO;

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);
            _abilitySO = (ApplyEffectToTargetPostNormalAttackPassive)abilitySO;
        }

        protected override void OnAttacked(Battle.Components.Character target)
        {
            if (FailedToActive()) return;
            RemovePreviousEffectIfExisted();
            var gameplayEffectContextHandle = new GameplayEffectContextHandle(SkillContext);
            var effectSpec = _abilitySO.Effect.CreateEffectSpec(Owner, gameplayEffectContextHandle);
            if (SkillContext.Parameters.targetAttribute.CaptureFrom == EGameplayEffectCaptureSource.Source)
                target = Character;
            _activeGameplayEffect = target.AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        private void RemovePreviousEffectIfExisted()
        {
            if (_activeGameplayEffect == null) return;
            _activeGameplayEffect.IsActive = false;
            Owner.EffectSystem.RemoveEffect(_activeGameplayEffect.Spec);
            _activeGameplayEffect = null;
        }

        private bool FailedToActive()
        {
            var rnd = Random.value;
            var failedToActive = rnd > _abilitySO.SuccessRate;
            Debug.Log(
                $"Failed to active {AbilitySO.name} with rate {_abilitySO.SuccessRate}, rolled {rnd}");
            return failedToActive;
        }
    }
}