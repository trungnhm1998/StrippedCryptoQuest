using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    [Serializable]
    public struct AttributeModifier
    {
        public AttributeScriptableObject Attribute;
        public Modifier Modifier;
    }

    /// <summary>
    /// This class is for working with effects without have to modify any existing code.
    /// </summary>
    [Serializable]
    public class EffectSpecificationContainer
    {
        private AbstractEffect _effectSpec;
        public AbstractEffect EffectSpec => _effectSpec;

        private AttributeModifier[] _modifiers = Array.Empty<AttributeModifier>();
        public AttributeModifier[] Modifiers => _modifiers;

        public EModifierType ModifierType => _effectSpec.EffectSO.EffectDetails.StackingType;
        public bool Expired => _effectSpec.IsExpired;
        public TagScriptableObject[] GrantedTags => _effectSpec.EffectSO.GrantedTags;

        public EffectSpecificationContainer(AbstractEffect effectSpec, bool generateModifiers = true)
        {
            _effectSpec = effectSpec;
            if (generateModifiers)
            {
                _modifiers = GenerateEffectModifier(_effectSpec);
            }
        }

        private AttributeModifier[] GenerateEffectModifier(AbstractEffect abstractEffect)
        {
            var modifierDefAfterCalculation = CalculateModifierMagnitude(abstractEffect);
            if (abstractEffect.EffectSO.ExecutionCalculation)
            {
                abstractEffect.EffectSO.ExecutionCalculation.ExecuteImplementation(ref abstractEffect,
                    ref modifierDefAfterCalculation);
            }

            var calculatedModifiers = new AttributeModifier[modifierDefAfterCalculation.Length];

            for (int index = 0; index < modifierDefAfterCalculation.Length; index++)
            {
                EffectAttributeModifier modifier = modifierDefAfterCalculation[index];
                var attribute = modifier.AttributeSO;
                var modifierValue = modifier.Value;
                var modType = modifier.ModifierType;

                var copiedModifier = new Modifier();
                switch (modType)
                {
                    case EAttributeModifierType.Add:
                        copiedModifier.Additive = modifierValue;
                        break;
                    case EAttributeModifierType.Multiply:
                        copiedModifier.Multiplicative = modifierValue;
                        break;
                    case EAttributeModifierType.Override:
                        copiedModifier.Overriding = modifierValue;
                        break;
                }

                calculatedModifiers[index] = new AttributeModifier()
                {
                    Attribute = attribute,
                    Modifier = copiedModifier
                };
            }

            return calculatedModifiers;
        }

        /// <summary>
        /// This will also create a snapshot/copy of the current attribute modifier
        /// Apply the modifier value using custom logic (e.g. level rate logic per level)
        /// </summary>
        /// <param name="abstractEffect"></param>
        /// <returns></returns>
        private EffectAttributeModifier[] CalculateModifierMagnitude(AbstractEffect abstractEffect)
        {
            var modifierDefsDetail = abstractEffect.EffectSO.EffectDetails.Modifiers;
            var calculatedModifiers = new EffectAttributeModifier[modifierDefsDetail.Length];

            for (int i = 0; i < calculatedModifiers.Length; i++)
            {
                EffectAttributeModifier modifier = modifierDefsDetail[i].Clone();
                if (modifier.ModifierComputationMethod)
                {
                    modifier.Value += modifier.ModifierComputationMethod.CalculateMagnitude(abstractEffect);
                }

                calculatedModifiers[i] = modifier;
            }

            return calculatedModifiers;
        }


        public void ClearModifiers()
        {
            _modifiers = Array.Empty<AttributeModifier>();
        }

        public void Update(float deltaTime)
        {
            _effectSpec.Update(deltaTime);
        }
    }
}