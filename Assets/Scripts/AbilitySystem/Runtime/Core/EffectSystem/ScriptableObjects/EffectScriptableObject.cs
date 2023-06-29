using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class EffectScriptableObject : ScriptableObject
    {
        [TextArea(0, 3)]
        public string Description;
        [Tooltip("Which attribute to affect and how it affected")]
        public EffectDetails EffectDetails;
        [Tooltip("When the effect is applied to a target, these tags will be granted through logic of AbilitySystem")]
        public TagScriptableObject[] GrantedTags;
        [Tooltip("These tags must be present or must not (ignore tags) for the effect to be applied.")]
        public TagRequireIgnoreDetails ApplicationTagRequirements;
        [Tooltip("How the effect being custom caculated. Can leave it null or DefaultSO.")]
        public AbstractEffectExecutionCalculationSO ExecutionCalculation;

        [Header("Application rules")]
        [Range(0f, 1f)] [Tooltip("There {ChanceToApply}*100% chance that effect will be applied")]
        public float ChanceToApply = 1f;
        [Tooltip("Custom requirement to know if we can apply effect or not")]
        public List<EffectCustomApplicationRequirement> CustomApplicationRequirements = new List<EffectCustomApplicationRequirement>();

        /// <summary>
        /// Get a new AbstractEffect instance with the given level and level rate. from this ScriptableObject.
        /// note level and level rate are specific to Mugen Horror Action game.
        /// </summary>
        /// <param name="abilitySystem"></param>
        /// <param name="levelRate"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual AbstractEffect GetEffect(AbilitySystemBehaviour ownerSystem, object origin, AbilityParameters parameters)
        {
            var effect = CreateEffect();
            effect.Origin = origin.ToString();
            effect.InitEffect(this, ownerSystem, parameters);
            return effect;
        }

        protected abstract AbstractEffect CreateEffect();

        public Action OnEffectApplied;
        public Action OnEffectActivated;
        public Action OnEffectDeactivated;
    }

    public abstract class EffectScriptableObject<T> : EffectScriptableObject where T : AbstractEffect, new()
    {
        protected override AbstractEffect CreateEffect() => new T();
    }
}