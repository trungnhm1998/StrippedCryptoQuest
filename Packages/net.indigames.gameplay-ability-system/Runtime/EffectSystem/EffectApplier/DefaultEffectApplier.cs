using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier
{
    public class DefaultEffectApplier : IEffectApplier
    {
        private readonly AbilitySystemBehaviour _ownerSystem;
        private readonly AttributeSystemBehaviour _attributeSystem;

        public DefaultEffectApplier(AbilitySystemBehaviour ownerSystem)
        {
            _ownerSystem = ownerSystem;
            _attributeSystem = ownerSystem.AttributeSystem;
        }

        /// <summary>
        /// It's mean instantly modify base of the attribute
        /// suitable for non-stats attribute like HP (not MaxHP)
        /// Attack can be treat as instant effect
        /// Enemy -> attack (effect) -> Player
        ///
        /// Based on GAS I would want Instant effect as a infinite effect but for now I will modify the base value
        /// </summary>
        public void Visit(InstantEffectSpec instantEffectSpec)
        {
            Debug.Log(
                $"DefaultEffectApplier::ApplyInstantEffect {instantEffectSpec.Def.name} to system {_ownerSystem.name}");
            var container = new ActiveEffectSpecification(instantEffectSpec);
            var computedModifiers = container.ComputedModifiers;

            for (int index = 0; index < computedModifiers.Count; index++)
            {
                var computedModifier = computedModifiers[index];
                var modifier = computedModifier.Modifier;
                var attribute = computedModifier.Attribute;

                // get a copy of the attribute value
                if (!_attributeSystem.TryGetAttributeValue(attribute, out var attributeValue)) continue;

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

                _attributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
                Debug.Log($"DefaultEffectApplier::ApplyInstantEffect" +
                          $"::to attribute {attribute} " +
                          $"base value[{attributeValue.BaseValue}] " +
                          $"currentValue[{attributeValue.CurrentValue}]");
            }
        }

        public void Visit(DurationalEffectSpec durationalEffectSpec) => InternalVisitDurational(durationalEffectSpec);

        public void Visit(InfiniteEffectSpec infiniteEffectSpec) => InternalVisitDurational(infiniteEffectSpec);

        /// <summary>
        /// Slow enemy down for 15sec
        /// We would want to add this active effect into the applied effect, when the effect is expired
        /// Remove it and recalculate the attribute modifiers will be easier
        /// </summary>
        private void InternalVisitDurational(GameplayEffectSpec effectSpec)
        {
            _ownerSystem.EffectSystem.AppliedEffects.Add(new ActiveEffectSpecification(effectSpec));
            Debug.Log(
                $"EffectApplier::Durational::to {_ownerSystem.name} with effect {effectSpec.Def.name}");
        }
    }
}