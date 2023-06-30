using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public class EffectApplier : IEffectApplier
    {
        private readonly AbilitySystemBehaviour _ownerSystem;
        private readonly AttributeSystemBehaviour _attributeSystem;

        public EffectApplier(AbilitySystemBehaviour ownerSystem)
        {
            _ownerSystem = ownerSystem;
            _attributeSystem = ownerSystem.AttributeSystem;
        }

        /// <summary>
        /// It's mean instantly modify base of the attribute
        /// suitable for non-stats attribute like HP (not MaxHP)
        /// Attack can be treat as instant effect
        /// Enemy -> attack (effect) -> Player
        /// </summary>
        /// <param name="abstractEffect"></param>
        public void ApplyInstantEffect(AbstractEffect abstractEffect)
        {
            Debug.Log($"EffectApplier::ApplyInstantEffect {abstractEffect.EffectSO.name} to system {_ownerSystem.name}");
            var container = new EffectSpecificationContainer(abstractEffect);
            var modifiers = container.Modifiers;

            var effectSO = abstractEffect.EffectSO;
            var effectAttributeModifiers = effectSO.EffectDetails.Modifiers;

            for (int index = 0; index < effectAttributeModifiers.Length; index++)
            {
                var modifierSpec = effectAttributeModifiers[index];
                var attribute = modifierSpec.AttributeSO;
                _attributeSystem.GetAttributeValue(attribute, out var attributeValue);

                Modifier calculatedModifier = modifiers[index].Modifier;
                switch (modifierSpec.ModifierType)
                {
                    case EAttributeModifierType.Add:
                        attributeValue.BaseValue += calculatedModifier.Additive;
                        break;
                    case EAttributeModifierType.Multiply:
                        attributeValue.BaseValue += attributeValue.BaseValue * calculatedModifier.Multiplicative;
                        break;
                    case EAttributeModifierType.Override:
                        attributeValue.BaseValue = calculatedModifier.Overriding;
                        break;
                }

                _attributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
                Debug.Log($"EffectApplier::ApplyInstantEffect::to attribute {attribute.name} base value {attributeValue.BaseValue} currentValue {attributeValue.CurrentValue}");
            }
            _ownerSystem.TagSystem.GrantedTags.AddRange(effectSO.GrantedTags);
        }

        /// <summary>
        /// Slow enemy down for 15sec
        /// </summary>
        /// <param name="abstractEffect"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ApplyDurationalEffect(AbstractEffect abstractEffect)
        {
            _ownerSystem.EffectSystem.AppliedEffects.Add(new EffectSpecificationContainer(abstractEffect));
            _ownerSystem.TagSystem.GrantedTags.AddRange(abstractEffect.EffectSO.GrantedTags);
            Debug.Log($"EffectApplier::Durational::to {_ownerSystem.name} with effect {abstractEffect.EffectSO.name}");
        }
    }
}