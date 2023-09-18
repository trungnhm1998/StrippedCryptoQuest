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
        public ActiveEffectSpecification Visit(InstantEffectSpec instantEffectSpecSpec)
        {
            Debug.Log(
                $"DefaultEffectApplier::ApplyInstantEffect {instantEffectSpecSpec.Def.name} to system {_ownerSystem.name}");
            var container = new ActiveEffectSpecification(instantEffectSpecSpec);
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
                          $"::to attribute {attribute.name} " +
                          $"base value[{attributeValue.BaseValue}] " +
                          $"currentValue[{attributeValue.CurrentValue}]");
            }

            // after modify the attribute this effect is now expired
            // The system only care if effect is expired or not
            container.IsActive = false;

            return container;
        }

        public ActiveEffectSpecification Visit(DurationalEffectSpec durationalEffectSpec) =>
            InternalVisitDurational(durationalEffectSpec);

        public ActiveEffectSpecification Visit(InfiniteEffectSpec infiniteEffectSpec) =>
            InternalVisitDurational(infiniteEffectSpec);

        /// <summary>
        /// Slow enemy down for 15sec
        /// We would want to add this active effect into the applied effect, when the effect is expired
        /// Remove it and recalculate the attribute modifiers will be easier
        /// </summary>
        private ActiveEffectSpecification InternalVisitDurational(GameplayEffectSpec effectSpec)
        {
            Debug.Log("EffectApplier::InternalVisitDurational::" +
                      $"Apply effect {effectSpec.Def.name} to {_ownerSystem.name}");
            return new ActiveEffectSpecification(effectSpec);
        }
    }
}