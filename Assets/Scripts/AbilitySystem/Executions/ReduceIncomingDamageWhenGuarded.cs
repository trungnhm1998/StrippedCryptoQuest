using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class ReduceIncomingDamageWhenGuarded : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var targetSystem = executionParams.TargetAbilitySystemComponent;
            var tagSystem = targetSystem.TagSystem;
            if (tagSystem == null)
            {
                Debug.LogError("TagSystemBehaviour not found on " + targetSystem.name);
                return;
            }

            if (tagSystem.HasTag(TagsDef.Guard) == false) return;

            for (int i = 0; i < outModifiers.Modifiers.Count; i++)
            {
                var modifier = outModifiers.Modifiers[i];
                if (modifier.Attribute != AttributeSets.Health) continue;
                outModifiers.Modifiers[i] = ReduceDamage(modifier);
            }
        }

        private GameplayModifierEvaluatedData ReduceDamage(GameplayModifierEvaluatedData modifier)
        {
            var reducedDamage = modifier.Magnitude / 2;

            Debug.Log($"Guard activated, reduce damage from: {modifier.Magnitude} to {reducedDamage}");

            return new GameplayModifierEvaluatedData()
            {
                Attribute = AttributeSets.Health,
                ModifierOp = EAttributeModifierOperationType.Add,
                Magnitude = Mathf.RoundToInt(reducedDamage)
            };
        }
    }
}