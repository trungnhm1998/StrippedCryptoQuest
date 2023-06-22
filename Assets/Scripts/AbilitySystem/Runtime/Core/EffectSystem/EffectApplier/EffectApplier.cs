using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public class EffectApplier : IEffectApplier
    {
        private readonly SkillSystem _skillSystem;
        private readonly AttributeSystem _attributeSystem;

        public EffectApplier(SkillSystem skillSystem)
        {
            _skillSystem = skillSystem;
            _attributeSystem = skillSystem.AttributeSystem;
        }

        /// <summary>
        /// Attack can be treat as instant effect
        /// Enemy -> attack (effect) -> Player
        /// </summary>
        /// <param name="abstractEffect"></param>
        public void ApplyInstantEffect(AbstractEffect abstractEffect)
        {
            Debug.Log($"EffectApplier::ApplyInstantEffect {abstractEffect.EffectSO.name} to system {_skillSystem.name}");
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
                        attributeValue.BaseValue *= calculatedModifier.Multiplicative;
                        break;
                }

                _attributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
                Debug.Log($"EffectApplier::ApplyInstantEffect::to attribute {attribute.name} base value {attributeValue.BaseValue} currentValue {attributeValue.CurrentValue}");
            }
            _skillSystem.GrantedTags.AddRange(effectSO.GrantedTags);
        }

        /// <summary>
        /// Slow enemy down for 15sec
        /// </summary>
        /// <param name="abstractEffect"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ApplyDurationalEffect(AbstractEffect abstractEffect)
        {
            _skillSystem.AppliedDurationalEffects.Add(new EffectSpecificationContainer(abstractEffect));

            Debug.Log($"EffectApplier::Durational::to {_skillSystem.name} with effect {abstractEffect.EffectSO.name}");
        }
    }
}