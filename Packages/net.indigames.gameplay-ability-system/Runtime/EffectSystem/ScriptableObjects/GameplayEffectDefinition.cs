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
        menuName = "Indigames Ability System/EffectSystem/GameplayEffectDefinition")]
    [Serializable]
    public class GameplayEffectDefinition : ScriptableObject
    {
        [field: SerializeField, TextArea(0, 3)]
        public string Description { get; private set; }

        /// <summary>
        /// How this effect will be applied to target
        /// - <see cref="DurationalPolicy"/>
        ///     Affect the attribute system for a duration
        /// - <see cref="InstantPolicy"/>
        ///     Change the base value of the attribute system
        /// - <see cref="PeriodicPolicy"/>
        ///     For every period, the effect will be applied to the target base value
        /// - <see cref="InfinitePolicy"/>
        ///     Affect the attribute system until the effect is removed
        /// </summary>
        [SerializeReference, SubclassSelector]
        private IGameplayEffectPolicy _policy = new InstantPolicy();
        public IGameplayEffectPolicy Policy
        {
            get => _policy;
            set => _policy = value;
        }

        [field: SerializeField, Tooltip("What attribute to affect and how it affected")]
        public EffectDetails EffectDetails { get; set; } = new();

        [field: SerializeField]
        public bool IsStack { get; private set; }

        [field: SerializeField]
        [Tooltip("When the effect is applied to a target, these tags will be granted through logic of AbilitySystem")]
        public TagScriptableObject[] GrantedTags { get; private set; } = Array.Empty<TagScriptableObject>();

        /// <summary>
        /// Tags on the Target that determine if a GameplayEffect can be applied to the Target.
        /// If these requirements are not met, the GameplayEffect is not applied.
        /// </summary>
        [field: SerializeField,
                Tooltip("These tags must be present or must not (ignore tags) for the effect to be applied.")]
        public TagRequireIgnoreDetails ApplicationTagRequirements { get; private set; } = new();

        /// <summary>
        /// GameplayEffects on the Target that have any of these tags in their Asset Tags or Granted Tags
        /// will be removed from the Target when this GameplayEffect is successfully applied.
        /// </summary>
        [field: SerializeField]
        public TagScriptableObject[] RemoveGameplayEffectsWithTags { get; private set; } =
            Array.Empty<TagScriptableObject>();

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
        public GameplayEffectSpec CreateEffectSpec(AbilitySystemBehaviour ownerSystem,
            GameplayEffectContextHandle context)
        {
            var effect = CreateEffect();
            effect.Context = context;
            effect.InitEffect(this, ownerSystem);
            return effect;
        }

        protected virtual GameplayEffectSpec CreateEffect() => new();
    }
}