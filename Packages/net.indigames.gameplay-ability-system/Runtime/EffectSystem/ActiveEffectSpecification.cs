using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    /// <summary>
    /// Represent an effect that being active on the system
    /// This is a wrapper for <see cref="GameplayEffectSpec"/> to store the computed modifier
    /// </summary>
    [Serializable]
    public class ActiveEffectSpecification
    {
        [Serializable]
        public struct ComputedModifier
        {
            public AttributeScriptableObject Attribute;
            public Modifier Modifier;
            public EAttributeModifierType ModifierType;

            public ComputedModifier(EffectAttributeModifier modifierDef)
            {
                Attribute = modifierDef.Attribute;
                ModifierType = modifierDef.ModifierType;
                Modifier = new Modifier();
                switch (ModifierType)
                {
                    case EAttributeModifierType.Add:
                        Modifier.Additive = modifierDef.Value;
                        break;
                    case EAttributeModifierType.Multiply:
                        Modifier.Multiplicative = modifierDef.Value;
                        break;
                    case EAttributeModifierType.Override:
                        Modifier.Overriding = modifierDef.Value;
                        break;
                }
            }
        }

        private IGameplayEffectSpec _effectSpec;
        public IGameplayEffectSpec EffectSpec => _effectSpec;

        /// <summary>
        /// After calculated magnitude and custom execution calculation
        /// The value would changed a new modifier would be added into the list
        /// </summary>
        [field: SerializeField] public List<ComputedModifier> ComputedModifiers { get; set; }

        public EModifierType ModifierType => _effectSpec.Def.EffectDetails.StackingType;
        public bool IsActive { get; set; } = true;
        public bool Expired => _effectSpec == null || _effectSpec.IsExpired || IsActive == false;
        public TagScriptableObject[] GrantedTags => _effectSpec.Def.GrantedTags;

        /// <summary>
        /// To prevent null reference exception
        /// </summary>
        public ActiveEffectSpecification() { }

        public ActiveEffectSpecification(IGameplayEffectSpec effectSpec)
        {
            _effectSpec = effectSpec;
            ComputedModifiers = GenerateEffectModifier(_effectSpec);
        }

        /// <summary>
        /// Calculated the modifier value based on the <see cref="ModifierComputationSO"/> and <see cref="EffectExecutionCalculationBase"/>
        /// </summary>
        /// <param name="effectSpec"></param>
        /// <returns></returns>
        private List<ComputedModifier> GenerateEffectModifier(IGameplayEffectSpec effectSpec)
        {
            var calculatedMagnitudeModifiers = CalculateModifierMagnitude(effectSpec);
            ExecuteCustomCalculations(effectSpec, ref calculatedMagnitudeModifiers);

            var computedModifiers = new List<ComputedModifier>();

            for (int index = 0; index < calculatedMagnitudeModifiers.Count; index++)
            {
                EffectAttributeModifier modifierDef = calculatedMagnitudeModifiers[index];
                computedModifiers.Add(new ComputedModifier(modifierDef));
            }

            return computedModifiers;
        }

        private void ExecuteCustomCalculations(IGameplayEffectSpec effectSpec,
            ref List<EffectAttributeModifier> calculatedMagnitudeModifiers)
        {
            var customCalculations = effectSpec.Def.ExecutionCalculations;
            for (int index = 0; index < customCalculations.Length; index++)
            {
                var customCalculation = customCalculations[index];
                if (customCalculation == null) continue;

                var executionParams = new CustomExecutionParameters(effectSpec);
                customCalculation.Execute(ref executionParams, ref calculatedMagnitudeModifiers);
            }
        }


        /// <summary>
        /// This will also create a snapshot/copy of the current attribute modifier
        /// Apply the modifier value using custom logic (e.g. level rate logic per level)
        /// 
        /// for now it will only return the original modifier value.
        /// 
        /// This is a legacy from Mugen Horror Project where effect has level rate
        /// </summary>
        /// <param name="gameplayEffectSpec"></param>
        /// <returns></returns>
        private List<EffectAttributeModifier> CalculateModifierMagnitude(IGameplayEffectSpec gameplayEffectSpec)
        {
            var effectDefModifiers = gameplayEffectSpec.Def.EffectDetails.Modifiers;
            var calculatedModifiers = new List<EffectAttributeModifier>();

            for (int i = 0; i < effectDefModifiers.Length; i++)
            {
                var modifier = effectDefModifiers[i].Clone(); // make sure to create a copy
                if (modifier.ModifierMagnitude)
                {
                    modifier.Value =
                        (modifier.ModifierMagnitude.CalculateMagnitude(gameplayEffectSpec) * modifier.Value)
                        .GetValueOrDefault();
                }

                calculatedModifiers.Add(modifier);
            }

            return calculatedModifiers;
        }


        public void ClearModifiers()
        {
            ComputedModifiers = new();
        }

        public void Update(float deltaTime)
        {
            _effectSpec.Update(deltaTime);
        }
    }
}