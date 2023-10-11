using System.Collections.Generic;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.EffectCalculations
{
    public class ReduceIncomingDamageWhenGuarded : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            var targetSystem = executionParams.TargetAbilitySystemComponent;
            var tagSystem = targetSystem.TagSystem;
            if (tagSystem == null)
            {
                Debug.LogError("TagSystemBehaviour not found on " + targetSystem.name);
                return;
            }

            if (tagSystem.HasTag(TagsDef.Guard) == false) return;

            for (int i = 0; i < outModifiers.Count; i++)
            {
                var modifier = outModifiers[i];
                if (modifier.Attribute != AttributeSets.Health) continue;
                outModifiers[i] = ReduceDamage(modifier);
            }
        }

        private EffectAttributeModifier ReduceDamage(EffectAttributeModifier modifier)
        {
            var newModifier = modifier.Clone();
            var reducedDamage = modifier.Value / 2;
            newModifier.Value = Mathf.RoundToInt(reducedDamage);

            Debug.Log($"Guard activated, reduce damage from: {modifier.Value} to {reducedDamage}");

            return newModifier;
        }
    }
}