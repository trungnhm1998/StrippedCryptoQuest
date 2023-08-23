using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.CustomApplicationRequirement;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects
{
    public abstract class EffectScriptableObject : ScriptableObject
    {
        [TextArea(0, 3)]
        public string Description;
        [Tooltip("Which attribute to affect and how it affected")]
        public EffectDetails EffectDetails;
        [Tooltip("When the effect is applied to a target, these tags will be granted through logic of AbilitySystem")]
        public TagScriptableObject[] GrantedTags = Array.Empty<TagScriptableObject>();
        [Tooltip("These tags must be present or must not (ignore tags) for the effect to be applied.")]
        public TagRequireIgnoreDetails ApplicationTagRequirements = new();
        [Tooltip("How the effect being custom caculated. Can leave it null or DefaultSO.")]
        public EffectExecutionCalculationBase ExecutionCalculation;
        public EffectExecutionCalculationBase[] ExecutionCalculations;

        [Header("Application rules")]
        [Range(0f, 1f)] [Tooltip("There {ChanceToApply}*100% chance that effect will be applied")]
        public float ChanceToApply = 1f;
        [Tooltip("Custom requirement to know if we can apply effect or not")]
        public List<EffectCustomApplicationRequirement> CustomApplicationRequirements = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerSystem"></param>
        /// <returns></returns>
        public virtual GameplayEffectSpec CreateEffectSpec(AbilitySystemBehaviour ownerSystem)
        {
            var effect = CreateEffect();
            effect.InitEffect(this, ownerSystem);
            return effect;
        }

        protected abstract GameplayEffectSpec CreateEffect();

        public Action OnEffectApplied;
        public Action OnEffectActivated;
        public Action OnEffectDeactivated;
    }

    public abstract class EffectScriptableObject<T> : EffectScriptableObject where T : GameplayEffectSpec, new()
    {
        protected override GameplayEffectSpec CreateEffect() => new T();
    }
}