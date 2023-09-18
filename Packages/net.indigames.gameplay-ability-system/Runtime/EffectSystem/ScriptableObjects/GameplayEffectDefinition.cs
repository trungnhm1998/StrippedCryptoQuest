using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.CustomApplicationRequirement;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplayEffectDefinition",
        menuName = "IndiGames/GameplayAbilitySystem/EffectSystem/GameplayEffectDefinition")]
    [Serializable]
    public class GameplayEffectDefinition : ScriptableObject
    {
        [field: SerializeField, TextArea(0, 3)]
        public string Description { get; private set; }

        /// <summary>
        /// How this effect will be applied to target
        /// - Durational
        ///     Affect the attribute system for a duration
        /// - Instant
        ///     Change the base value of the attribute system
        /// - Periodic
        ///     For every period, the effect will be applied to the target base value
        /// - Infinite
        ///     Affect the attribute system until the effect is removed
        /// </summary>
        [field: SerializeField]
        public GameplayEffectActionBase EffectActionBase { get; set; }

        [field: SerializeField, Tooltip("What attribute to affect and how it affected")]
        public EffectDetails EffectDetails { get; set; } = new();

        [field: SerializeField]
        [Tooltip("When the effect is applied to a target, these tags will be granted through logic of AbilitySystem")]
        public TagScriptableObject[] GrantedTags { get; private set; } = Array.Empty<TagScriptableObject>();

        [field: SerializeField,
                Tooltip("These tags must be present or must not (ignore tags) for the effect to be applied.")]
        public TagRequireIgnoreDetails ApplicationTagRequirements { get; private set; } = new();

        /// <summary>
        /// e.g. Need to calculate based on caster's attribute
        /// </summary>
        [field: SerializeField,
                Tooltip("How the effect being calculated with custom logic. Can leave it null or DefaultSO.")]
        public EffectExecutionCalculationBase[] ExecutionCalculations { get; private set; } =
            Array.Empty<EffectExecutionCalculationBase>();

        [field: SerializeField, Header("Application rules"), Range(0f, 1f)]
        [Tooltip("There {ChanceToApply}*100% chance that effect will be applied")]
        public float ChanceToApply { get; private set; } = 1f;

        [field: SerializeField, Tooltip("Custom requirement to know if we can apply effect or not")]
        public List<EffectCustomApplicationRequirement> CustomApplicationRequirements { get; private set; } = new();

        /// <summary>
        /// Create a new Specification from this Definition
        /// </summary>
        /// <param name="ownerSystem">from owner</param>
        /// <returns></returns>
        public virtual GameplayEffectSpec CreateEffectSpec(AbilitySystemBehaviour ownerSystem)
        {
            var effect = CreateEffect();
            effect.InitEffect(this, ownerSystem);
            return effect;
        }

        protected virtual GameplayEffectSpec CreateEffect() => new();
    }
}