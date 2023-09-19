using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.Helper;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    /// <summary>
    /// A specification represent an effect that created from <see cref="GameplayEffectDefinition"/>
    /// When the effect is applied to the system, it will create a new <see cref="ActiveEffectSpecification"/> to store the computed modifier
    /// Every effect should create from <see cref="GameplayEffectDefinition.CreateEffectSpec"/>
    /// </summary>
    [Serializable]
    public class GameplayEffectSpec : IComparable<GameplayEffectSpec>
    {
        /// <summary>
        /// The system give/apply this effect
        /// </summary>
        public AbilitySystemBehaviour Source { get; set; }

        /// <summary>
        /// The system this effect applying to. Can be the same as <see cref="Source"/> and it will be self apply effect
        /// </summary>
        public AbilitySystemBehaviour Target { get; set; }

        public EffectDetails EffectDefDetails { get; set; }
        public TagScriptableObject[] GrantedTags { get; set; }
        public EffectExecutionCalculationBase[] ExecutionCalculations { get; set; }

        public bool IsExpired { get; set; }

        public ModifierSpec[] Modifiers = Array.Empty<ModifierSpec>();

        /// <summary>
        /// Which Data/SO the effect is based on
        /// </summary>
        public GameplayEffectDefinition Def { get; private set; }

        public void InitEffect(GameplayEffectDefinition effectDef, AbilitySystemBehaviour source)
        {
            Source = source;

            if (effectDef == null)
            {
                Debug.LogWarning("EffectSO is null");
                return;
            }

            Def = effectDef;
            EffectDefDetails = Def.EffectDetails;
            GrantedTags = Def.GrantedTags;
            ExecutionCalculations = Def.ExecutionCalculations;

            Modifiers = new ModifierSpec[Def.EffectDetails.Modifiers.Length];

            OnInitEffect(effectDef, source);
        }

        public virtual void OnInitEffect(GameplayEffectDefinition def, AbilitySystemBehaviour source) { }

        public bool CanApply()
        {
            if (Source == null) return false;
            if (!CheckTagRequirementsMet()) return false;

            if (Def == null)
            {
                Debug.LogWarning("GameplayEffectSpec::canApply::Tried to apply effect with no Def.");
                return false;
            }

            var effectSODetails = Def.EffectDetails;

            for (var index = 0; index < effectSODetails.Modifiers.Length; index++)
            {
                var modifier = effectSODetails.Modifiers[index];
                if (!modifier.Attribute)
                {
                    Debug.LogWarning(
                        $"GameplayEffectSpec::canApply::Effect {Def.name} has a modifier with no Attribute at idx[{index}].");
                    return false;
                }
            }

            var rndValue = Random.value;
            if (rndValue > Def.ChanceToApply)
            {
                Debug.Log(
                    $"{Def.name} failed to apply with chance {Def.ChanceToApply} and random value {rndValue}");
                return false;
            }

            for (var index = 0; index < Def.CustomApplicationRequirements.Count; index++)
            {
                var appReq = Def.CustomApplicationRequirements[index];
                if (appReq == null || appReq.CanApplyEffect(Def, this, Source)) continue;
                Debug.Log($"{appReq} doesn't meet the requirement for {Def.name}");
                return false;
            }

            return true;
        }


        protected virtual bool CheckTagRequirementsMet()
        {
            var tagConditionDetail = Def.ApplicationTagRequirements;
            return AbilitySystemHelper.SystemHasAllTags(Source, tagConditionDetail.RequireTags)
                   && AbilitySystemHelper.SystemHasNoneTags(Source, tagConditionDetail.IgnoreTags);
        }

        public void CalculateModifierMagnitudes()
        {
            var effectSODetails = Def.EffectDetails;
            for (var index = 0; index < effectSODetails.Modifiers.Length; index++)
            {
                var modifierDef = effectSODetails.Modifiers[index];
                var modifierSpec = Modifiers[index];

                var modifierMagnitude = modifierDef.ModifierMagnitude;
                if (modifierMagnitude == null)
                {
                    Debug.LogWarning(
                        $"GameplayEffectSpec::calculateModifierMagnitudes::Effect {Def.name} has a modifier with no ModifierMagnitude at idx[{index}].");
                    modifierSpec.EvaluatedMagnitude = modifierDef.Value; // TODO: THIS IS A CHEAT
                    Modifiers[index] = modifierSpec;
                    continue;
                }

                if (modifierMagnitude.AttemptCalculateMagnitude(this, out modifierSpec.EvaluatedMagnitude) == false)
                {
                    modifierSpec.EvaluatedMagnitude = 0;
                    Debug.Log($"Modifier on spec {Def.name} failed to calculate magnitude. Falling back to 0.");
                }


                Modifiers[index] = modifierSpec;
            }
        }

        public bool IsValid() => Def != null;
        public int CompareTo(GameplayEffectSpec other) => Def != other.Def ? 0 : 1;

        public ActiveEffectSpecification CreateActiveEffectSpec(AbilitySystemBehaviour owner)
            => Def.EffectAction.CreateActiveEffect(this, owner);
    }

    /// <summary>
    /// Wrapper for <see cref="GameplayEffectDefinition.EffectDetails.Modifiers"/>
    /// </summary>
    [Serializable]
    public struct ModifierSpec
    {
        public float EvaluatedMagnitude;

        private GameplayEffectSpec _effectSpec;
        private ActiveEffectSpecification _activeEffectSpecification;

        public float GetEvaluatedMagnitude() => EvaluatedMagnitude;
    }
}