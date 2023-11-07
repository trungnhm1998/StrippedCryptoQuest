using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    /// <summary>
    /// This calculation should be placed after some damage calculation and 
    /// Before damage event, effect,...
    /// </summary>
    public class CriticalCalculation : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            if (executionParams.SourceAbilitySystemComponent.TryGetComponent(
                    out Battle.Components.Character character) == false) return;

            for (var index = 0; index < outModifiers.Modifiers.Count; index++)
            {
                var outMod = outModifiers.Modifiers[index];
                if (!IsDamageModifier(outMod)) continue;
                if (!character.AttributeSystem.TryGetAttributeValue(AttributeSets.CriticalRate,
                    out var criticalRate)) continue;

                var rand = Random.value;

                if (rand <= criticalRate.CurrentValue / 100f)
                {
                    outMod.Magnitude *= BaseBattleVariable.CRITICAL_FACTOR;
                    outMod.Magnitude = Mathf.RoundToInt(outMod.Magnitude);
                    outModifiers.Modifiers[index] = outMod;
                    
                    BattleEventBus.RaiseEvent(new CriticalHitEvent()
                    {
                        Character = character,
                    });
                }
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"CriticalCalculation::Critical check: {rand} : {criticalRate.CurrentValue / 100f}" + 
                    $"\nOccurred: {rand <= criticalRate.CurrentValue / 100f} Out damage: {outMod.Magnitude}");
#endif
            }
        }

        private bool IsDamageModifier(GameplayModifierEvaluatedData data)
        {
            return data.Attribute == AttributeSets.Health && data.Magnitude < 0;
        }
    }
}