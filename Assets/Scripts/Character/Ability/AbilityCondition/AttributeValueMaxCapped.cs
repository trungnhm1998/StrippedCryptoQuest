using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Helper;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;


namespace CryptoQuest.Character.Ability.AbilityCondition
{
    public class AttributeValueMaxCapped : IAbilityCondition
    {
        public AttributeValueMaxCapped() { }

        public bool IsPass(AbilityConditionContext ctx)
        {
            var effectDef = ctx.EffectDef;
            var system = ctx.System;
            if (effectDef == null || system == null) return false;

            for (int i = 0; i < effectDef.EffectDetails.Modifiers.Length; i++)
            {
                var modDef = effectDef.EffectDetails.Modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modDef.ModifierType != EAttributeModifierType.Add) continue;
                if (modDef.Attribute == null) continue;
                if (!system.AttributeSystem.TryGetAttributeValue(modDef.Attribute, out var attributeValue)) continue;
                
                if (modDef.Value < 0) return true; // TODO: Debug using bomb on self

                var attributeWithCapped = modDef.Attribute as AttributeWithMaxCapped;
                if (attributeWithCapped == null) continue;
                system.AttributeSystem.TryGetAttributeValue(attributeWithCapped.CappedAttribute, out var cappedValue);
                if (attributeValue.CurrentValue < cappedValue.CurrentValue)
                {
                    return true;
                }
            }

            Debug.Log($"All attribute already at max value");

            return false;
        }
    }
}