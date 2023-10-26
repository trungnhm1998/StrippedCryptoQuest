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
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }
        [field: SerializeField] public GameplayEffectContext Context { get; private set; }

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
            RemovePreviousEffectIfExisted();
            var gameplayEffectContextHandle = new GameplayEffectContextHandle(_abilitySO.Context);
            var effectSpec = _abilitySO.Effect.CreateEffectSpec(Owner, gameplayEffectContextHandle);
            if (_abilitySO.Context.Parameters.targetAttribute.CaptureFrom == EGameplayEffectCaptureSource.Source)
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
    }
}