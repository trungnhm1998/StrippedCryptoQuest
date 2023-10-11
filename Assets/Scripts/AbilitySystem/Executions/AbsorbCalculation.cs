using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using CoreEffectContext = IndiGames.GameplayAbilitySystem.EffectSystem.GameplayEffectContext;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class AbsorbCalculation : EffectExecutionCalculationBase
    {
        /// <summary>
        /// This will contains the exec cal for absorb calculation
        /// 
        /// either this or create at runtime
        /// </summary>
        [SerializeField] private GameplayEffectDefinition _absorbEffectDefinition;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var context = GameplayEffectContext.ExtractEffectContext(executionParams.EffectSpec.Context);
            var sourceSystem = executionParams.SourceAbilitySystemComponent;
            if (sourceSystem.TagSystem.HasTag(TagsDef.Absorb) == false) return;

            // find the damage from out modifier
            float damage = 0f;
            foreach (var evaluatedMod in outModifiers.Modifiers)
            {
                if (evaluatedMod.Attribute != AttributeSets.Health) continue;
                damage += evaluatedMod.Magnitude;
            }

            var absorbContext = new DamageDealtContext(damage, context.SkillInfo);
            var absorbSpec = _absorbEffectDefinition.CreateEffectSpec(sourceSystem, new GameplayEffectContextHandle(absorbContext));
            sourceSystem.ApplyEffectSpecToSelf(absorbSpec);
        }
    }

    public class DamageDealtContext : GameplayEffectContext
    {
        public float Damage { get; private set; }

        public DamageDealtContext(float damage, SkillInfo skillInfo) : base(skillInfo)
        {
            Damage = damage;
        }
    }
}