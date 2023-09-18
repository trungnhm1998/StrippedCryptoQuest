using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation
{
    public enum EGameplayEffectCaptureSource
    {
        /// <summary>
        /// Where the effect created from 
        /// </summary>
        Source,

        /// <summary>
        /// Where the effect will be applied to
        /// </summary>
        Target
    }

    /// <summary>
    /// Use this to quickly setup a capture def for attribute and where to capture it from.
    /// </summary>
    [Serializable]
    public struct CustomExecutionAttributeCaptureDef
    {
        public AttributeScriptableObject Attribute;
        public EGameplayEffectCaptureSource CaptureFrom;
    }

    public struct CustomExecutionParameters
    {
        public AbilitySystemBehaviour TargetAbilitySystemComponent { get; private set; }
        public AbilitySystemBehaviour SourceAbilitySystemComponent { get; private set; }
        public GameplayEffectSpec EffectSpec { get; private set; }

        public CustomExecutionParameters(GameplayEffectSpec effectSpec)
        {
            EffectSpec = effectSpec;
            TargetAbilitySystemComponent = effectSpec.Target;
            SourceAbilitySystemComponent = effectSpec.Source;
        }

        /// <summary>
        /// Get the attribute value from the defined source
        /// 
        /// TODO: Should calculate the magnitude
        /// </summary>
        /// <param name="captureAttributeDef"></param>
        /// <param name="attributeValue"></param>
        /// <param name="defaultCurrentValue">When <see cref="AttributeValue.CurrentValue"/> == 0, get the default</param>
        public void TryGetAttributeValue(CustomExecutionAttributeCaptureDef captureAttributeDef,
            out AttributeValue attributeValue, float defaultCurrentValue = 0f)
        {
            attributeValue = new AttributeValue();
            switch (captureAttributeDef.CaptureFrom)
            {
                case EGameplayEffectCaptureSource.Source:
                    SourceAbilitySystemComponent.AttributeSystem.TryGetAttributeValue(captureAttributeDef.Attribute,
                        out attributeValue);
                    break;
                case EGameplayEffectCaptureSource.Target:
                    TargetAbilitySystemComponent.AttributeSystem.TryGetAttributeValue(captureAttributeDef.Attribute,
                        out attributeValue);
                    break;
            }

            if (attributeValue.CurrentValue == 0f && defaultCurrentValue != 0f)
            {
                attributeValue.CurrentValue = defaultCurrentValue;
            }
        }
    }

    /// <summary>
    /// Override this to create custom logic for calculating the effect modifiers before it is applied to the target.
    /// </summary>
    public abstract class EffectExecutionCalculationBase : ScriptableObject
    {
        /// <summary>
        /// Custom logic for calculating the effect modifier before it is applied to the target.
        /// such as calculate the damage based on the target's defense and owner's attack damage.
        /// by default this will do nothing
        /// 
        /// For case attack that depends on the source damage and target defends
        /// this would add a new -HP modifier with Add type to the list
        /// </summary>
        /// <param name="executionParams"></param>
        /// <param name="outModifiers"></param>
        public abstract void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers);
    }
}