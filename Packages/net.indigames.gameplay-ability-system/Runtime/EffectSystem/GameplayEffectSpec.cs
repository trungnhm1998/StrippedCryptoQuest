using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Helper;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    public interface IGameplayEffectSpec
    {
        public EffectScriptableObject Def { get; }
        public bool IsExpired { get; }
        public AbilitySystemBehaviour Target { get; }
        public AbilitySystemBehaviour Source { get; }
        public void Update(float deltaTime);
    }

    [Serializable]
    public partial class GameplayEffectSpec : IGameplayEffectSpec
    {
        /// <summary>
        /// Which Data/SO the effect is based on
        /// </summary>
        public EffectScriptableObject Def { get; private set; }

        /// <summary>
        /// The system give/apply this effect
        /// </summary>
        public AbilitySystemBehaviour Source { get; set; }

        /// <summary>
        /// The system this effect applying to. Can be the same as <see cref="Source"/> and it will be self apply effect
        /// </summary>
        public AbilitySystemBehaviour Target { get; set; }

        public bool IsExpired { get; set; }
        public bool RemoveWhenAbilityEnd = true;

        public ModifierSpec[] Modifiers = Array.Empty<ModifierSpec>();

        protected IEffectApplier _effectApplier;

        public virtual void InitEffect(EffectScriptableObject effectDef)
            => InitEffect(effectDef, null);

        public virtual void InitEffect(EffectScriptableObject effectDef, AbilitySystemBehaviour source)
        {
            Source = source;

            if (effectDef == null)
            {
                Debug.LogWarning("EffectSO is null");
                return;
            }

            Def = effectDef;
            Modifiers = new ModifierSpec[Def.EffectDetails.Modifiers.Length];
        }

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
                if (appReq != null && !appReq.CanApplyEffect(Def, this, Source))
                {
                    Debug.Log($"{appReq} doesn't meet the requirement for {Def.name}");
                    return false;
                }
            }

            return true;
        }

        public virtual ActiveEffectSpecification Accept(IEffectApplier effectApplier)
        {
            _effectApplier = effectApplier;
            return new ActiveEffectSpecification();
        }

        public virtual void Update(float deltaTime) { }

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
    }

    [Serializable]
    public struct ModifierSpec
    {
        public float EvaluatedMagnitude;

        private GameplayEffectSpec _effectSpec;
        private ActiveEffectSpecification _activeEffectSpecification;

        public float GetEvaluatedMagnitude() => EvaluatedMagnitude;
    }

    [Obsolete]
    public class NullEffectSpec : GameplayEffectSpec
    {
        private static NullEffectSpec _instance;

        public static NullEffectSpec Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NullEffectSpec();
                return _instance;
            }
        }

        private NullEffectSpec()
        {
            IsExpired = true;
        }

        public override void Update(float deltaTime) { }
    }
}