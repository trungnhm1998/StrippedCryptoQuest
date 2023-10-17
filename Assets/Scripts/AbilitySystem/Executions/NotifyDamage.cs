using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class NotifyDamage : EffectExecutionCalculationBase
    {
        public event Action<Battle.Components.Character, float> DamageDealt;
        
        [SerializeField] private AttributeScriptableObject _healthAttribute;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            foreach (var mod in outModifiers.Modifiers)
            {
                if (mod.Attribute != _healthAttribute) continue;
                if (executionParams.TargetAbilitySystemComponent.TryGetComponent(
                        out Battle.Components.Character character) == false) return;
                DamageDealt?.Invoke(character, mod.Magnitude);
            }
        }
    }
}