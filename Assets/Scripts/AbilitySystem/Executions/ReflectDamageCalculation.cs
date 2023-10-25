using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class ReflectContext : IndiGames.GameplayAbilitySystem.EffectSystem.GameplayEffectContext
    {
        public Battle.Components.Character Receiver { get; }
        public Battle.Components.Character Dealer { get; }
        public float Damage { get; }
        public float ReflectDamagePercentage { get; }

        public ReflectContext(Battle.Components.Character ctxReceiver, Battle.Components.Character ctxDealer,
            float ctxDamage, float reflectDamagePercentage)
        {
            Receiver = ctxReceiver;
            Dealer = ctxDealer;
            Damage = ctxDamage;
            ReflectDamagePercentage = reflectDamagePercentage;
        }
    }

    public class ReflectDamageCalculation : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var baseContext = spec.Context.Get();
            if (baseContext is not ReflectContext context) return;

            var reflectDamage = context.Damage * (context.ReflectDamagePercentage / 100f);

            outModifiers.Add(new GameplayModifierEvaluatedData(
                AttributeSets.Health,
                EAttributeModifierOperationType.Add,
                -Mathf.RoundToInt(reflectDamage)
            ));
        }
    }
}