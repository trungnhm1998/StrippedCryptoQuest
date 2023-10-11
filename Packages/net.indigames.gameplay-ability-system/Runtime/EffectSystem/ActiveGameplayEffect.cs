using System;
using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    /// <summary>
    /// Represent an effect that being active on the system
    /// This is a wrapper for <see cref="GameplayEffectSpec"/> to store the computed modifier
    /// - What GameplayEffectSpec
    /// - Start Time
    /// - When To execute next (periodic instant effect)
    /// </summary>
    [Serializable]
    public class ActiveGameplayEffect
    {
        private GameplayEffectSpec _spec;
        private AttributeSystemBehaviour TargetAttributeSystem => _spec.Target.AttributeSystem;
        public GameplayEffectSpec Spec => _spec;

        /// <summary>
        /// Calculated modifiers after <see cref="EffectExecutionCalculationBase.Execute"/>
        /// </summary>
        private List<GameplayModifierEvaluatedData> _computedModifiers = new();
        public List<GameplayModifierEvaluatedData> ComputedModifiers => _computedModifiers;

        public EModifierType ModifierType => _spec.EffectDefDetails.StackingType;
        public bool IsActive { get; set; } = true;
        public bool Expired => _spec == null || _spec.IsExpired || IsActive == false;
        public TagScriptableObject[] GrantedTags => _spec.GrantedTags;

        /// <summary>
        /// To prevent null reference exception
        /// </summary>
        public ActiveGameplayEffect() { }

        /// <summary>
        /// Currently only support capture the modifier when the effect is created
        ///
        /// For case such as periodic effect with up to date modifier, we need to update the modifier
        /// </summary>
        /// <param name="spec"></param>
        public ActiveGameplayEffect(GameplayEffectSpec spec)
        {
            _spec = spec;
            ExecuteCustomCalculations(_spec, out _computedModifiers);
        }

        private void ExecuteCustomCalculations(GameplayEffectSpec effectSpec,
            out List<GameplayModifierEvaluatedData> evaluatedDatas)
        {
            var customCalculations = effectSpec.ExecutionCalculations;
            GameplayEffectCustomExecutionOutput output = new();
            for (int index = 0; index < customCalculations.Length; index++)
            {
                var customCalculation = customCalculations[index];
                if (customCalculation == null) continue;

                var executionParams = new CustomExecutionParameters(effectSpec);
                customCalculation.Execute(ref executionParams, ref output);
            }

            evaluatedDatas = output.Modifiers;
        }

        public virtual void Update(float deltaTime) { }

        public bool IsValid()
            => _spec != null && _spec.IsValid() && _spec.IsExpired == false && IsActive;

        public bool HasTag(TagScriptableObject tag) => _spec.GrantedTags.Contains(tag);

        public void UpdateStackCount(GameplayEffectSpec inSpec)
        {
            _spec.StackCount++;
            OnSpecStackChanged(inSpec);
        }

        /// <summary>
        /// Call back when stack changes, derived for custom logic when stack changed
        /// </summary>
        /// <param name="otherSpec"></param>
        protected virtual void OnSpecStackChanged(GameplayEffectSpec otherSpec) { }

        /// <summary>
        /// How is this active effect modify the target system
        /// - Infinite and Duration effect would add modifier to the system
        /// - Instant effect would modify the base value of the attribute
        /// - Periodic will check if the period interval and apply the effect just like instant effect
        /// </summary>
        public virtual void ExecuteActiveEffect()
        {
            if (IsActive == false) return;
            
            ApplyModifiersUsingEffectDefDetails();
            ApplyModifiersUsingComputedExecCal();
        }

        private void ApplyModifiersUsingEffectDefDetails()
        {
            // apply modifier using def first
            for (var index = 0; index < _spec.Def.EffectDetails.Modifiers.Length; index++)
            {
                var modifier = _spec.Def.EffectDetails.Modifiers[index];
                AddModifierToAttribute(modifier.Attribute, modifier.OperationType, _spec.GetModifierMagnitude(index));
            }
        }

        private void ApplyModifiersUsingComputedExecCal()
        {
            // apply modifier after execute custom calculation
            foreach (var modifier in _computedModifiers)
                AddModifierToAttribute(modifier.Attribute, modifier.ModifierOp, modifier.Magnitude);
        }

        private void AddModifierToAttribute(AttributeScriptableObject attribute, EAttributeModifierOperationType opType, float magnitude)
        {
            var modToApply = new Modifier();
            switch (opType)
            {
                case EAttributeModifierOperationType.Add:
                    modToApply.Additive = magnitude;
                    break;
                case EAttributeModifierOperationType.Multiply:
                    modToApply.Multiplicative = magnitude;
                    break;
                case EAttributeModifierOperationType.Override:
                    modToApply.Overriding = magnitude;
                    break;
            }

            _spec.Target.AttributeSystem.TryAddModifierToAttribute(modToApply, attribute, ModifierType);
        }

        /// <summary>
        /// The case is we have an effect with modifier want to affect an attribute that is not in the system yet
        ///
        /// e.g. Modifier to increase gold drop rate, but the attribute system does not have gold drop rate attribute.
        /// We can either add the attribute to the system or this method would add it for us. only at runtime
        /// </summary>
        private void AddAttributeToSystemIfNotExists(AttributeScriptableObject attribute)
        {
            if (!TargetAttributeSystem.HasAttribute(attribute, out _))
                TargetAttributeSystem.AddAttribute(attribute);
        }

        protected bool InternalExecuteMod(GameplayEffectSpec spec, GameplayModifierEvaluatedData modEvalData)
        {
            GameplayEffectModCallbackData executeData = new(spec, modEvalData, spec.Source);
            var targetSystem = spec.Target.EffectSystem;

            /*
             *  This should apply 'gamewide' rules. Such as clamping Health to MaxHealth or granting +3 health
             * for every point of strength, etc
             */
            if (targetSystem.PreGameplayEffectExecute(executeData) == false) return false;

            ModifyAttributeBaseValue(modEvalData.Attribute, modEvalData.ModifierOp, modEvalData.Magnitude, ref executeData);

            targetSystem.PostGameplayEffectExecute(executeData);

            return true;
        }

        public void ModifyAttributeBaseValue(AttributeScriptableObject attribute, EAttributeModifierOperationType modifierOp,
            float magnitude, ref GameplayEffectModCallbackData executeData)
        {
            var targetAttributeSystem = Spec.Target.AttributeSystem;
            if (!targetAttributeSystem.TryGetAttributeValue(attribute, out var curBase)) return;
            float newBase = StaticExecModOnBaseValue(curBase.BaseValue, modifierOp, magnitude);
            targetAttributeSystem.SetAttributeBaseValue(attribute, newBase);

            Debug.Log($"ActiveGameplayEffect::ModifyBaseAttribute [{attribute.name}]" +
                      $" old base value: [{curBase.BaseValue}] " +
                      $" new base value: [{newBase}]");
        }

        private static float StaticExecModOnBaseValue(float baseValue, EAttributeModifierOperationType opType, float magnitude)
        {
            switch (opType)
            {
                case EAttributeModifierOperationType.Add:
                    return baseValue + magnitude;
                case EAttributeModifierOperationType.Multiply:
                    return baseValue * magnitude;
                case EAttributeModifierOperationType.Division:
                    if (magnitude.NearlyEqual(0) == false)
                        return baseValue / magnitude;
                    break;
                case EAttributeModifierOperationType.Override:
                    return magnitude;
                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }

            return baseValue;
        }
    }
}