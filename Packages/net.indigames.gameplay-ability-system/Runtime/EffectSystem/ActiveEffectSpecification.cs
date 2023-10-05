using System;
using System.Collections.Generic;
using System.Linq;
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
            public ModifierComputationSO ModifierComputation;

            public ComputedModifier(EffectAttributeModifier modifierDef)
            {
                ModifierComputation = modifierDef.ModifierMagnitude;
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

        [SerializeField] private GameplayEffectSpec _effectSpec;
        public GameplayEffectSpec EffectSpec => _effectSpec;

        /// <summary>
        /// After calculated magnitude and custom execution calculation
        /// The value would changed a new modifier would be added into the list
        /// </summary>
        [field: SerializeField] public List<ComputedModifier> ComputedModifiers { get; set; }

        public EModifierType ModifierType => _effectSpec.EffectDefDetails.StackingType;
        public bool IsActive { get; set; } = true;
        public bool Expired => _effectSpec == null || _effectSpec.IsExpired || IsActive == false;
        public TagScriptableObject[] GrantedTags => _effectSpec.GrantedTags;

        /// <summary>
        /// To prevent null reference exception
        /// </summary>
        public ActiveEffectSpecification() { }

        public ActiveEffectSpecification(GameplayEffectSpec effectSpec)
        {
            _effectSpec = effectSpec;
            ComputedModifiers = GenerateEffectModifier(_effectSpec);
        }

        /// <summary>
        /// Calculated the modifier value based on the <see cref="ModifierComputationSO"/> and <see cref="EffectExecutionCalculationBase"/>
        /// </summary>
        /// <param name="effectSpec"></param>
        /// <returns></returns>
        private List<ComputedModifier> GenerateEffectModifier(GameplayEffectSpec effectSpec)
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

        private void ExecuteCustomCalculations(GameplayEffectSpec effectSpec,
            ref List<EffectAttributeModifier> calculatedMagnitudeModifiers)
        {
            var customCalculations = effectSpec.ExecutionCalculations;
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
        private List<EffectAttributeModifier> CalculateModifierMagnitude(GameplayEffectSpec gameplayEffectSpec)
        {
            var effectDefModifiers = gameplayEffectSpec.EffectDefDetails.Modifiers;
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

        public virtual void Update(float deltaTime) { }

        public bool IsValid()
            => _effectSpec != null && _effectSpec.IsValid() && _effectSpec.IsExpired == false && IsActive;

        public bool HasTag(TagScriptableObject tag) => _effectSpec.GrantedTags.Contains(tag);

        /// <summary>
        /// By default instant, duration and infinite effect can be applied to attribute system
        ///
        /// Periodic should only apply internally when the interval is reached
        /// </summary>
        /// <returns></returns>
        public virtual bool CanApplyModifiersToAttributeSystem() => true;

        public void Release()
        {
            OnRelease();
        }

        protected virtual void OnRelease() { }
    }
}