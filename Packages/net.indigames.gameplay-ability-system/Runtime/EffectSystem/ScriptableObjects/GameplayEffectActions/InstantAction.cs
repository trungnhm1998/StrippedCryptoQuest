using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public class InstantAction : GameplayEffectActionBase
    {
        /// <summary>
        /// It's mean instantly modify base of the attribute
        /// suitable for non-stats attribute like HP (not MaxHP)
        /// Attack can be treat as instant effect
        /// Enemy -> attack (effect) -> Player
        ///
        /// Based on GAS I would want Instant effect as a infinite effect but for now I will modify the base value
        /// </summary>
        public override ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner)
        {
            Debug.Log(
                $"DefaultEffectApplier::ApplyInstantEffect {inSpec.Def.name} to system {owner.name}");
            var container = new ActiveEffectSpecification(inSpec);
            var computedModifiers = container.ComputedModifiers;
            var attributeSystem = owner.AttributeSystem;

            for (int index = 0; index < computedModifiers.Count; index++)
            {
                var computedModifier = computedModifiers[index];
                var modifier = computedModifier.Modifier;
                var attribute = computedModifier.Attribute;

                // get a copy of the attribute value
                if (!attributeSystem.TryGetAttributeValue(attribute, out var attributeValue)) continue;

                switch (computedModifier.ModifierType)
                {
                    case EAttributeModifierType.Add:
                        attributeValue.BaseValue += modifier.Additive;
                        break;
                    case EAttributeModifierType.Multiply:
                        attributeValue.BaseValue += attributeValue.BaseValue * modifier.Multiplicative;
                        break;
                    case EAttributeModifierType.Override:
                        attributeValue.BaseValue = modifier.Overriding;
                        break;
                }

                attributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
                Debug.Log($"DefaultEffectApplier::ApplyInstantEffect" +
                          $"::to attribute {attribute.name} " +
                          $"base value[{attributeValue.BaseValue}] " +
                          $"currentValue[{attributeValue.CurrentValue}]");
            }

            // after modify the attribute this effect is now expired
            // The system only care if effect is expired or not
            container.IsActive = false;

            return container;
        }
    }
}