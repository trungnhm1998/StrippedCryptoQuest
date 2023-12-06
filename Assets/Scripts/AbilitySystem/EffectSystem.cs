using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem
{
    public class EffectSystem : EffectSystemBehaviour
    {
        private readonly HashSet<GameplayEffectDefinition> _effectWithLargestMagnitude = new();

        public override void UpdateAttributeSystemModifiers()
        {
            _effectWithLargestMagnitude.Clear();
            AttributeSystem.ResetAttributeModifiers();
            foreach (var effect in AppliedEffects.Where(effect => !effect.Expired))
            {
                effect.Spec.CalculateModifierMagnitudes();
                var context = GameplayEffectContext.ExtractEffectContext(effect.Spec.Context);
                if (context == null || context.SkillInfo.SkillType == ESkillType.Passive)
                {
                    effect.ExecuteActiveEffect();
                    continue;
                }
                if (_effectWithLargestMagnitude.Contains(effect.Spec.Def)) continue;
                var largestMagnitudeEffect = GetLargestGameplayEffectMagnitude(effect.Spec);
                _effectWithLargestMagnitude.Add(largestMagnitudeEffect.Spec.Def);
                largestMagnitudeEffect.ExecuteActiveEffect();
            }
        }

        public ActiveGameplayEffect GetLargestGameplayEffectMagnitude(GameplayEffectSpec spec)
        {
            var largestMagnitudeEffect = new ActiveGameplayEffect();
            foreach (var effect in AppliedEffects)
            {
                if (effect.IsValid() == false) continue;
                if (effect.Spec.Def != spec.Def) continue;
                if (!largestMagnitudeEffect.IsValid())
                {
                    largestMagnitudeEffect = effect;
                    continue;
                }

                for (var index = 0; index < effect.ComputedModifiers.Count; index++)
                {
                    var otherEffectEvaluatedMod = effect.ComputedModifiers[index];
                    var currentLargestEffectEvaluatedMod = largestMagnitudeEffect.ComputedModifiers[index];
                    if (Mathf.Abs(otherEffectEvaluatedMod.Magnitude) >
                        Mathf.Abs(currentLargestEffectEvaluatedMod.Magnitude))
                    {
                        Debug.Log(
                            $"Largest magnitude effect is {largestMagnitudeEffect.Spec.Def.name} with {otherEffectEvaluatedMod.Attribute}: {otherEffectEvaluatedMod.Magnitude}");
                        largestMagnitudeEffect = effect;
                        break;
                    }
                }
            }


            return largestMagnitudeEffect;
        }
    }
}